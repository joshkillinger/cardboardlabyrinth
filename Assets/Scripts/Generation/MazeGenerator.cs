using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public int MinRooms = 8;
    public int MaxRooms = 12;
    public float BranchOdds = .2f;

    public GameObject RoomDebugPrefab;
    public GameObject LineDebugPrefab;
    public GameObject RoomPrefab;

    public List<Room> Maze;

    public int MinX, MinY, MaxX, MaxY;

    public Text StatusText;
    public GameObject[] DestroyOnLoad;

    public bool DebugOn = false;

    private GameObject slider;

    private Queue<int> bfsQueue;

    // Use this for initialization
    void Start()
    {
        Room.MinX = MinX;
        Room.MinY = MinY;
        Room.MaxX = MaxX;
        Room.MaxY = MaxY;
        Room.Generator = this;

        slider = GameObject.Find("ProgressSlider");

        StartCoroutine(GenerateMaze());
    }

    IEnumerator GenerateMaze()
    {
        Room start = new Room(0, 0, 0);
        start.Entrance = true;
        Maze.Add(start);

        while (Maze.Count < MinRooms)
        {
            yield return CreateRoom(Maze[Random.Range(0, Maze.Count)]);
        }

        if (DebugOn)
        {
            Debug.Log("Number of rooms generated: " + Maze.Count);
        }


        yield return FindExit();

        Progress("Building Rooms", 0f);

        yield return Spawn();
    }

    /// <summary>
    /// Creates a new room or connection to an existing room
    /// </summary>
    /// <param name="parent">Room to spawn from</param>
    IEnumerator CreateRoom(Room parent)
    {
        Room.Direction[] directions = parent.AvailableDirections();
        if (directions.Length == 0)
        {
            yield return null;
        }
        else
        {
            int direction = Random.Range(0, directions.Length);

            Room r = RoomExists(parent.X, parent.Y, directions[direction]);
            if (r != null)
            {
                parent.Add(r, directions[direction]);
                r.Add(parent, Room.ReverseDirection(directions[direction]));
            }
            else
            {
                if (Maze.Count < MaxRooms)
                {
                    yield return AddRoom(parent, directions[direction]);
                }
            }
        }
    }

    /// <summary>
    /// Adds a room to the maze, and potentially spawns more rooms
    /// </summary>
    /// <param name="parent">Room that spawned this room</param>
    /// <param name="direction">Direction from <paramref name="parent"/></param>
    IEnumerator AddRoom(Room parent, Room.Direction direction)
    {
        Room r = new Room(parent, direction, Maze.Count);
        Maze.Add(r);

        //update the UI
        float progress = (float)Maze.Count / (float)MaxRooms;
        Progress("", progress / 2);

        int i = 0;
        while (i < 4)
        {
            if (Random.Range(0f, 1f) < BranchOdds)
                yield return CreateRoom(r);
            i++;
        }
    }

    /// <summary>
    /// Returns the reference to the room that exists in the maze
    /// </summary>
    /// <param name="x">Starting x</param>
    /// <param name="y">Starting y</param>
    /// <param name="direction">Offset direction from staring location</param>
    /// <returns>Reference to existing room, null if does not exist</returns>
    Room RoomExists(float x, float y, Room.Direction direction)
    {
        Room r = null;
        if (direction == Room.Direction.North)
            y += 1;
        else if (direction == Room.Direction.South)
            y -= 1;
        else if (direction == Room.Direction.East)
            x += 1;
        else if (direction == Room.Direction.West)
            x -= 1;

        //Debug.Log("Checking for room at " + x + ", " + y);

        for (int i = 0; i < Maze.Count; i++)
        {
            if ((Maze[i].X == x) && (Maze[i].Y == y))
            {
                r = Maze[i];
                break;
            }
        }

        return r;
    }

    /// <summary>
    /// Uses BFS to find the room farthest from the entrance, and marks it as the exit.
    /// </summary>
    IEnumerator FindExit()
    {
        int minX = Maze[0].X;
        int minY = Maze[0].Y;
        int maxX = Maze[0].X;
        int maxY = Maze[0].Y;

        foreach (Room r in Maze)
        {
            r.Distance = int.MaxValue;
            r.Visited = false;
            if (r.X < minX)
                minX = r.X;
            if (r.Y < minY)
                minY = r.Y;
            if (r.X > maxX)
                maxX = r.X;
            if (r.Y > maxY)
                maxY = r.Y;
        }

        float count = 0f;

        bfsQueue = new Queue<int>();
        bfsQueue.Enqueue(0);
        Maze[0].Distance = 0;

        while (bfsQueue.Count > 0)
        {
            int r = bfsQueue.Dequeue();
            Maze[r].Visited = true;

            foreach (int child in Maze[r].Neighbors)
            {
                if (child < 0)
                    continue;
                if (Maze[child].Visited)
                    continue;
                if (bfsQueue.Contains(child))
                    continue;

                Maze[child].Distance = Maze[r].Distance + 1;
                bfsQueue.Enqueue(child);
            }

            count += 1;
            float progress = (count / Maze.Count) / 2;
            Progress("", .5f + progress);

            yield return null;
        }

        int maxDistance = 0;
        int farthestRoom = 0;
        foreach (Room r in Maze)
        {
            if ((r.X == minX) || (r.X == maxX) || (r.Y == minY) || (r.Y == maxY))
            {
                if (r.Distance > maxDistance)
                {
                    maxDistance = r.Distance;
                    farthestRoom = r.Index;
                }
            }
        }
        Maze[farthestRoom].Exit = true;
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPaused = true;
//#endif
    }

    /// <summary>
    /// Instantiates the rooms in the maze based on piecemeal prefabs
    /// </summary>
    IEnumerator Spawn()
    {
        for (int i = 0; i < Maze.Count; i++)
        {
            Room r = Maze[i];

            Vector3 pos = new Vector3(r.X * 10, 0, r.Y * 10);

            if (DebugOn)
            {
                GameObject g = GameObject.Instantiate(RoomDebugPrefab, pos, Quaternion.identity) as GameObject;
                for (int j = 0; j < 4; j++)
                {
                    if (r.Neighbors[j] >= 0)
                    {
                        GameObject l = GameObject.Instantiate<GameObject>(LineDebugPrefab);
                        LineRenderer lr = l.GetComponent<LineRenderer>();
                        lr.SetPosition(1, Room.DirectionToVector3((Room.Direction)j) * 10);
                        l.transform.SetParent(g.transform);
                        l.transform.localPosition = new Vector3(0, 0, 0);
                    }
                }
            }
            else
            {
                GameObject g = GameObject.Instantiate(RoomPrefab, pos, Quaternion.identity) as GameObject;
                RoomBuilder builder = g.GetComponent<RoomBuilder>();
                builder.RoomSetup = r;
                builder.Construct();
            }

            Progress("", (float)i / (float)Maze.Count);
            yield return null;
        }

        LoadScene();
    }

    /// <summary>
    /// Additively loads the maze scene and destroys the list of unnecessary objects
    /// </summary>
    private void LoadScene()
    {
        SceneManager.LoadScene("maze", LoadSceneMode.Additive);

        foreach (GameObject g in DestroyOnLoad)
        {
            GameObject.Destroy(g);
        }
    }

    private void Progress(string text, float progress)
    {
        if (text.Length > 0)
        {
            StatusText.text = text;
        }

        Debug.Log("Updating progress bar to " + progress);
        ExecuteEvents.Execute<IProgressUpdate>(slider, null, (x, y) => x.UpdateProgress(progress));
    }


}