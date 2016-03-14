using UnityEngine;
using System.Collections;

public class PlayerNavigation : MonoBehaviour
{
    public float MoveAccelleration = 2;
    public float MoveSpeed = 2;

    public bool DebugMessages = false;

    private float currentSpeed;

    private Vector3 destination;
    private float startMoveTime;
    private bool moving = false;

    // Use this for initialization
    void Start()
    {
        destination = new Vector3();
        startMoveTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Move();
        }
    }

    public void SetDestination(Vector3 dest)
    {
        destination = dest;
        moving = true;
    }

    private void Move()
    {
        float moveDist = MoveSpeed * Time.deltaTime;
        Vector3 direction = destination - gameObject.transform.position;

        if ((moveDist * moveDist) > (Vector3.SqrMagnitude(direction)))
        {
            gameObject.transform.position = destination;
            moving = false;
        }
        else
        {
            direction.Normalize();
            gameObject.transform.Translate(direction * moveDist);
        }
    }
}