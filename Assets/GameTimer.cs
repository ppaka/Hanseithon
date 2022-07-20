using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float startTime;

    public float Time => UnityEngine.Time.realtimeSinceStartup - startTime;
    public float TimeAsMs => (UnityEngine.Time.realtimeSinceStartup - startTime) * 1000;


    private void Awake()
    {
        startTime = UnityEngine.Time.realtimeSinceStartup;
    }
}