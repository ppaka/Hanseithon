using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public void OpenStageScene()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
