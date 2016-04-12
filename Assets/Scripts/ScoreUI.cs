using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    public Text ScoreText;
    public Text Size;
    public Text Distance;

    // Use this for initialization
    void Start()
    {
        GameObject scoreBin = GameObject.Find("ScoreBin");
        Score score = scoreBin.GetComponent<Score>();

        ScoreText.text += score.Time.ToString("F1") + "s";
        Size.text += score.Rooms + " rooms";
        Distance.text += score.Distance + " rooms";

        GameObject.Destroy(scoreBin);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("generation");
    }
}