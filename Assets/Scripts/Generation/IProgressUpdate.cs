using UnityEngine.EventSystems;

public interface IProgressUpdate : IEventSystemHandler
{
    void UpdateProgress(float progress);
}