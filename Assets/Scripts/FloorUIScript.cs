using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FloorUIScript : MonoBehaviour
{
    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Respawn()
    {
        SceneManager.LoadScene("generation");
    }
}
