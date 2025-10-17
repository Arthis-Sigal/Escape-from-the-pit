using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolumeManager : MonoBehaviour
{
    [Header("Références")]
    public AudioMixer masterMixer;   
    public Slider volumeSlider;      

    private const string volumeParam = "MasterVolume"; 
    private const string playerPrefKey = "MasterVolumeValue";

    void Start()
    {
        // get the volume saved in player perfs
        float savedVolume = PlayerPrefs.GetFloat(playerPrefKey, 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        // Get the slider data
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        masterMixer.SetFloat(volumeParam, dB);

        PlayerPrefs.SetFloat(playerPrefKey, value);
    }
}
