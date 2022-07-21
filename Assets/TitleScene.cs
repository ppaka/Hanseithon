using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void OpenStageScene()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
