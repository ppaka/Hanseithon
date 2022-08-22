using UnityEngine;

public static class GameManager
{
    public static string CurrentScene;
    public static string LevelPath;
    public static float audioVolume;

    public static void LoadVolume()
    {
        audioVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    public static void SaveVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", audioVolume);
        PlayerPrefs.Save();
    }
}