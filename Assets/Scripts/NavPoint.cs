using UnityEngine;
using System.Collections;

public class NavPoint : MonoBehaviour
{
    public bool DebugMessages = false;

    PlayerNavigation player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerNavigation>();
    }

    public void Triggered()
    {
        if (DebugMessages)
        {
            Debug.Log(gameObject.transform.parent.name + " got triggered event");
        }
        player.SetDestination(gameObject.transform.parent.transform.position);
    }

    public void Entered(bool entered)
    {
        if (DebugMessages)
        {
            Debug.Log(gameObject.transform.parent.name + " got " + (entered ? "Entered" : "Exited") + " event");
        }
    }
}