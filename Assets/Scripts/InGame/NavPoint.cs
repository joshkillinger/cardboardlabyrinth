using UnityEngine;
using System.Collections;

public class NavPoint : MonoBehaviour
{
    public bool DebugMessages = false;

    PlayerNavigation player;

    void Start()
    {
        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        while (player == null)
        {
            GameObject g = GameObject.FindGameObjectWithTag("Player");
            if (g != null)
                player = g.GetComponent<PlayerNavigation>();
            yield return new WaitForSeconds(0.1f);
        }
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