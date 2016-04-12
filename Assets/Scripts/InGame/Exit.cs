using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour
{
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

    void OnTriggerEnter(Collider other)
    {
        player.ReachedExit();
    }
}