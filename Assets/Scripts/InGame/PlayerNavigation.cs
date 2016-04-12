using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerNavigation : MonoBehaviour
{
    public float MoveAccelleration = 2;
    public float MoveSpeed = 2;

    public float FadeTime = 3;
    public Image[] FadeImages;

    public bool DebugMessages = false;

    private float currentSpeed;

    private Vector3 destination;
    private float startMoveTime;
    private bool moving = false;

    private float startTime;
    private bool timing;

    private bool done = false;

    // Use this for initialization
    void Start()
    {
        destination = new Vector3();
        startMoveTime = Time.time;

        //clean up the old generation scene
        Scene gen = SceneManager.GetSceneByName("generation");
        Scene maze = SceneManager.GetSceneByName("maze");
        SceneManager.SetActiveScene(maze);
        SceneManager.MergeScenes(gen, maze);
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Move();
        }
    }

    /// <summary>
    /// If the player is not moving, sets a new destination and starts moving
    /// </summary>
    /// <param name="dest">World position to move to</param>
    public void SetDestination(Vector3 dest)
    {
        if (moving || done)
            return;

        destination = dest;
        moving = true;

        if (!timing)
        {
            startTime = Time.time;
            timing = true;
        }
    }

    /// <summary>
    /// Moves the player to the specified destination
    /// </summary>
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

    /// <summary>
    /// Inform the player that the exit has been reached, and ends the level
    /// </summary>
    public void ReachedExit()
    {
        float duration = Time.time - startTime;
        StartCoroutine(EndLevel(duration));
    }

    /// <summary>
    /// Fades the screen to black and ends the level
    /// </summary>
    private IEnumerator EndLevel(float completedTime)
    {
        float start = Time.time;
        while ((Time.time - start) < FadeTime)
        {
            float alpha = (Time.time - start)/FadeTime;
            Debug.Log("Alpha is now " + alpha);
            foreach(Image i in FadeImages)
            {
                Color c = i.color;
                c.a = alpha;
                i.color = c;
            }
            yield return null;
        }
    }
}