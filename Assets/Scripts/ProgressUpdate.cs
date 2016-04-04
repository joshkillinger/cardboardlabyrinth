using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Slider))]
public class ProgressUpdate : MonoBehaviour, IProgressUpdate
{
    Slider progressSlider;

    // Use this for initialization
    void Start()
    {
        progressSlider = GetComponent<Slider>();
    }

    public void UpdateProgress(float progress)
    {
        progressSlider.value = progress;
    }
}