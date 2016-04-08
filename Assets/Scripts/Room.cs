using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Room
{
    [System.NonSerialized]
    private int[] neighbors = { -1, -1, -1, -1 };
    public int[] Neighbors
    {
        get { return neighbors; }
        set { }
    }

    public bool Entrance = false;
    public bool Exit = false;
    public int X = 0;
    public int Y = 0;

    public static int MinX, MinY, MaxX, MaxY;
    public static MazeGenerator Generator;
    
    private int index;
    public int Index
    {
        get { return index; }
        set {}
    }

    public enum Direction
    {
        North = 0,
        South = 1,
        East = 2,
        West = 3
    }

    
    /// <summary>
    /// Instantiates a Room object at the specified location
    /// </summary>
    public Room(int x, int y, int index)
    {
        X = x;
        Y = y;
        this.index = index;
    }

    /// <summary>
    /// Instantiates a Room object with a connection to the parent. Creates connections in both Rooms.
    /// </summary>
    /// <param name="parent">Room that spawned this Room</param>
    /// <param name="direction">Direction from <paramref name="parent"/> to spawn this Room.</param>
    public Room(Room parent, Direction direction, int index)
    {
        X = parent.X;
        Y = parent.Y;
        
        this.index = index;

        if (direction == Direction.North)
        {
            Y = parent.Y + 1;
        }
        else if (direction == Direction.South)
        {
            Y = parent.Y - 1;
        }
        else if (direction == Direction.East)
        {
            X = parent.X + 1;
        }
        else
        {
            X = parent.X - 1;
        }

        parent.Add(this, direction);
        neighbors[(int)ReverseDirection(direction)] = parent.Index;

        //Debug.Log("Added new room to the " + direction + " at " + X + ", " + Y);
    }

    /// <summary>
    /// Gets an array of available directions for a hallway
    /// </summary>
    /// <returns>Directions available for spawning</returns>
    public Direction[] AvailableDirections()
    {
        List<Direction> directions = new List<Direction>();

        if (Y < MaxY)
        {
            if (neighbors[(int)Direction.North] < 0)
            {
                directions.Add(Direction.North);
            }
        }
        if (Y > MinY)
        {
            if (neighbors[(int)Direction.South] < 0)
            {
                directions.Add(Direction.South);
            }
        }
        if (X < MaxX)
        {
            if (neighbors[(int)Direction.East] < 0)
            {
                directions.Add(Direction.East);
            }
        }
        if (X > MinX)
        {
            if (neighbors[(int)Direction.West] < 0)
            {
                directions.Add(Direction.West);
            }
        }

        return directions.ToArray();
    }

    /// <summary>
    /// Add the specified room as a connection to this one
    /// </summary>
    /// <param name="other">Room to add</param>
    /// <param name="direction">Direction to add room</param>
    public void Add(Room other, Direction direction)
    {
        neighbors[(int)direction] = other.Index;
    }

    /// <summary>
    /// Reverses a direction
    /// </summary>
    public static Direction ReverseDirection(Direction d)
    {
        int r = (int)d ^ 0x1;
        return (Direction)r;
    }

    /// <summary>
    /// Converts a direction into a Vector3, with North = (0, 0, 1) and East = (1, 0, 0)
    /// </summary>
    public static Vector3 DirectionToVector3(Direction d)
    {
        if (d == Direction.North)
            return Vector3.forward;
        else if (d == Direction.South)
            return Vector3.back;
        else if (d == Direction.East)
            return Vector3.right;
        else
            return Vector3.left;
    }
}