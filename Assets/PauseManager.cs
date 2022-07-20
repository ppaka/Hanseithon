using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject obj, settingsObj;

    public void Open()
    {
        obj.SetActive(true);
        Time.timeScale = 0;
    }

    public void Close()
    {
        obj.SetActive(false);
        Time.timeScale = 1;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OpenOption()
    {
        settingsObj.SetActive(true);
    }
    
    public void CloseOption()
    {
        settingsObj.SetActive(false);
    }
}
