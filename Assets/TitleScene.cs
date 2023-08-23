using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public GameObject exitButton;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            exitButton.SetActive(false);
        }
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
