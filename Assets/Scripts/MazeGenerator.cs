using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public int MaxRooms = 10;
    public float BranchOdds = .2f;

    public GameObject RoomPrefab;
    public GameObject LinePrefab;

    public List<Room> maze;// = new List<Room>();

    // Use this for initialization
    void Start()
    {

        Room start = new Room(0, 0);
        start.Entrance = true;
        maze.Add(start);

        CreateRoom(start);

        Debug.Log("Number of rooms generated: " + maze.Count);

        Spawn();
    }

    /// <summary>
    /// Creates a new room or connection to an existing room
    /// </summary>
    /// <param name="parent">Room to spawn from</param>
    void CreateRoom(Room parent)
    {
        if (maze.Count == MaxRooms)
        {
            //Debug.Log("Max rooms generated, returning");
            return;
        }

        Room.Direction[] directions = parent.AvailableDirections();
        if (directions.Length == 0)
        {
            return;
        }

        int direction = Random.Range(0, directions.Length);

        Room r = RoomExists(parent.X, parent.Y, directions[direction]);
        if (r != null)
        {
            Debug.Log("Adding connection " + directions[direction] + " of " + parent.X + ", " + parent.Y);
            parent.Add(r, directions[direction]);
            r.Add(parent, Room.ReverseDirection(directions[direction]));
        }
        else
        {
            Debug.Log("Adding Room " + directions[direction] + " of " + parent.X + ", " + parent.Y);
            AddRoom(parent, directions[direction]);
        }
    }

    /// <summary>
    /// Adds a room to the maze, and potentially spawns more rooms
    /// </summary>
    /// <param name="parent">Room that spawned this room</param>
    /// <param name="direction">Direction from <paramref name="parent"/></param>
    void AddRoom(Room parent, Room.Direction direction)
    {
        Room r = new Room(parent, direction);
        maze.Add(r);

        int i = 0;
        while (i < 4)
        {
            if (Random.Range(0f, 1f) < BranchOdds)
                CreateRoom(r);
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

        Debug.Log("Checking for room at " + x + ", " + y);

        for (int i = 0; i < maze.Count; i++)
        {
            if ((maze[i].X == x) && (maze[i].Y == y))
            {
                r = maze[i];
                break;
            }
        }

        return r;
    }

    void Spawn()
    {
        foreach (Room r in maze)
        {
            
            Vector3 pos = new Vector3(r.X * 10, 0, r.Y * 10);
            Debug.Log("Instantiating room at " + pos); 
            GameObject g = GameObject.Instantiate(RoomPrefab, pos, Quaternion.identity) as GameObject;
            for (int i = 0; i < 4; i ++)
            {
                if (r.Neighbors[i] != null)
                {
                    GameObject l = GameObject.Instantiate<GameObject>(LinePrefab);
                    LineRenderer lr = l.GetComponent<LineRenderer>();
                    lr.SetPosition(1, Room.DirectionToVector3((Room.Direction)i)* 10);
                    l.transform.SetParent(g.transform);
                    l.transform.localPosition = new Vector3(0, 0, 0);
                }
            }
        }
    }
}