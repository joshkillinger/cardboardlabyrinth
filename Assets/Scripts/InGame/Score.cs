using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
    public float Time;
    public int Rooms;
    public int Distance;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}