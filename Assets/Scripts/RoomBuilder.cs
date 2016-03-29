using UnityEngine;
using System.Collections;

public class RoomBuilder : MonoBehaviour
{
    public Room RoomSetup;

    public Transform[] Walls = new Transform[4];
    public Transform[] Corners = new Transform[4];

    public GameObject[] WallPrefabs;
    public GameObject[] CornerPrefabs;

    public void Construct()
    {
        for (int i = 0; i < 4; i++)
        {
            if (RoomSetup.Neighbors[i] == null)
            {
                GameObject wall = GameObject.Instantiate<GameObject>(WallPrefabs[Random.Range(0, WallPrefabs.Length)]);
                wall.transform.SetParent(Walls[i], false);
            }

            GameObject corner = GameObject.Instantiate<GameObject>(CornerPrefabs[Random.Range(0, CornerPrefabs.Length)]);
            corner.transform.SetParent(Corners[i], false);
        }
    }
}