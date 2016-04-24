using UnityEngine;
using System.Collections;

public class RoomBuilder : MonoBehaviour
{
    public Room RoomSetup;

    public Transform[] Walls = new Transform[4];
    public Transform[] Corners = new Transform[4];

    public GameObject[] WallPrefabs0;
    public GameObject[] WallPrefabs1;
    public GameObject[] CornerPrefabs0to0;
    public GameObject[] CornerPrefabs0to1;
    public GameObject[] CornerPrefabs1to0;
    public GameObject[] CornerPrefabs1to1;
    public GameObject[] DecoPrefabs;
    public GameObject ExitPrefab;
    public GameObject Pillar4xPrefab;

    public float DecoChance = .2f;

    public void Construct()
    {
        int[] wallThickness = { Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2) };

        //build this room's walls and corners
        for (int i = 0; i < 4; i++)
        {
            //walls
            if (RoomSetup.Neighbors[i] < 0)
            {
                GameObject[] wallsToPickFrom = (wallThickness[i] == 0) ? WallPrefabs0 : WallPrefabs1;

                GameObject wall = GameObject.Instantiate<GameObject>(wallsToPickFrom[Random.Range(0, wallsToPickFrom.Length)]);
                wall.transform.SetParent(Walls[i], false);
            }

            //corners
            int left = wallThickness[i];
            int right = wallThickness[(i + 1) % 4];

            GameObject[] cornerToPickFrom;
            if (left < right)
            {
                cornerToPickFrom = CornerPrefabs0to1;
            }
            else if (left > right)
            {
                cornerToPickFrom = CornerPrefabs1to0;
            }
            else if ((right == 1) && (left == 1))
            {
                cornerToPickFrom = CornerPrefabs1to1;
            }
            else
            {
                 cornerToPickFrom = CornerPrefabs0to0;
            }

            GameObject corner = GameObject.Instantiate<GameObject>(cornerToPickFrom[Random.Range(0, cornerToPickFrom.Length)]);
            corner.transform.SetParent(Corners[i], false);
        }

        //check for exit
        if (RoomSetup.Exit)
        {
            GameObject exit = GameObject.Instantiate<GameObject>(ExitPrefab);
            exit.transform.SetParent(transform, false);

            GameObject pillars = GameObject.Instantiate<GameObject>(Pillar4xPrefab);
            pillars.transform.SetParent(transform, false);
        }
        else if (RoomSetup.Entrance)
        {
            GameObject pillars = GameObject.Instantiate<GameObject>(Pillar4xPrefab);
            pillars.transform.SetParent(transform, false);
        }
        //maybe add some decoration
        else if (Random.Range(0.0f, 1.0f) < DecoChance)
        {
            float angle = 90 * Random.Range(0, 4);

            GameObject deco = GameObject.Instantiate<GameObject>(DecoPrefabs[Random.Range(0, DecoPrefabs.Length)]);
            deco.transform.SetParent(transform, false);

            deco.transform.Rotate(Vector3.up, angle, Space.Self);
        }
    }
}