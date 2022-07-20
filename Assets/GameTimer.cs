using System.Collections;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float startTime;
    public AudioSource audioSource;
    public bool gameStarted;
    public GameObject pauseButton;
    public TMP_Text countText;

    public void ResetTimer()
    {
        startTime = UnityEngine.Time.timeSinceLevelLoad;
    }

    public float Time => UnityEngine.Time.timeSinceLevelLoad - startTime;
    public float TimeAsMs => (UnityEngine.Time.timeSinceLevelLoad - startTime) * 1000;


    private void Start()
    {
        UnityEngine.Time.timeScale = 0;
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        countText.gameObject.SetActive(true);
        countText.text = "준비";
        yield return new WaitForSecondsRealtime(2f);
        countText.text = "3";
        yield return new WaitForSecondsRealtime(1f);
        countText.text = "2";
        yield return new WaitForSecondsRealtime(1f);
        countText.text = "1";
        yield return new WaitForSecondsRealtime(1f);
        countText.text = "시작!";
        yield return new WaitForSecondsRealtime(1f);
        countText.gameObject.SetActive(false);
        StartGame();
    }

    private void StartGame()
    {
        gameStarted = true;
        UnityEngine.Time.timeScale = 1;
        ResetTimer();
        audioSource.Play();
        pauseButton.SetActive(true);
    }
}