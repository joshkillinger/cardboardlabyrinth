﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Room
{
    private Room[] neighbors = { null, null, null, null };
    public Room[] Neighbors
    {
        get { return neighbors; }
        set { }
    }

    public bool Entrance = false;
    public bool Exit = false;
    public int X = 0;
    public int Y = 0;

    public enum Direction
    {
        North = 0,
        South = 1,
        East = 2,
        West = 3
    }

    public Room(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Room(Room parent, Direction direction)
    {
        X = parent.X;
        Y = parent.Y;

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
        neighbors[(int)ReverseDirection(direction)] = parent;

        Debug.Log("Added new room to the " + direction + " at " + X + ", " + Y);
    }

    /// <summary>
    /// Gets an array of available directions for a hallway
    /// </summary>
    /// <returns>Directions available for spawning</returns>
    public Direction[] AvailableDirections()
    {
        List<Direction> directions = new List<Direction>();

        if (neighbors[(int)Direction.North] == null)
        {
            directions.Add(Direction.North);
        }
        if (Y > 0)
        {
            if (neighbors[(int)Direction.South] == null)
            {
                directions.Add(Direction.South);
            }
            
        }
        if (neighbors[(int)Direction.East] == null)
        {
            directions.Add(Direction.East);
        }
        if (X > 0)
        {
            if (neighbors[(int)Direction.West] == null)
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
        neighbors[(int)direction] = other;
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