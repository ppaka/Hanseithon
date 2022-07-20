using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(GameManager.CurrentScene);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelect");
    }
}