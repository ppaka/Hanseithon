using UnityEngine;
using UnityEngine.UI;

public class InGameSettings : MonoBehaviour
{
    public AudioSource source;
    public Slider slider;

    private void Start()
    {
        GameManager.LoadVolume();
        source.volume = GameManager.audioVolume;
        slider.SetValueWithoutNotify(GameManager.audioVolume);
    }

    public void OnChangedValue(float value)
    {
        GameManager.audioVolume = value;
        source.volume = value;
    }

    private void OnDestroy()
    {
        GameManager.SaveVolume();
    }
}