using UnityEngine;
using UnityEngine.UI;

public class TitleSettings : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        GameManager.LoadVolume();
        slider.SetValueWithoutNotify(GameManager.audioVolume);
    }

    public void OnChangedValue(float value)
    {
        GameManager.audioVolume = value;
    }

    private void OnDestroy()
    {
        GameManager.SaveVolume();
    }
}
