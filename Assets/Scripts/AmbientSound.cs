using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class AmbientSound : MonoBehaviour
{
    public AudioClip ambientClip1;
    [Range(0f, 1f)]
    public float volume1 = 0.5f;

    public AudioClip ambientClip2;
    [Range(0f, 1f)]
    public float volume2 = 0.5f;

    public AudioClip EndMenuClip;
    [Range(0f, 1f)]
    public float volumeEndMenu = 0.1f;

    private AudioSource source1;
    private AudioSource source2;
    public AudioMixerGroup masterGroup;

    private AudioSource EndMenuSource;

    private int currentLevel;

    void Start()
    {
        ambientClip1.LoadAudioData();
        ambientClip2.LoadAudioData();
        EndMenuClip.LoadAudioData();

 
        source1 = gameObject.AddComponent<AudioSource>();
        source1.clip = ambientClip1;
        source1.outputAudioMixerGroup = masterGroup;
        source1.volume = volume1;
        source1.loop = true;
        source1.Play();

        source2 = gameObject.AddComponent<AudioSource>();
        source2.clip = ambientClip2;
        source2.outputAudioMixerGroup = masterGroup;
        source2.volume = volume2;
        source2.loop = true;
        source2.Play();


        currentLevel = SceneManager.GetActiveScene().buildIndex;
        volumeEndMenu = 0.1f;
        volumeEndMenu = volumeEndMenu + (currentLevel * 0.1f);


    }

    void Update()
    {
        GameObject endPanel = GameObject.Find("CanvasEndingMenu");

        if (endPanel != null && endPanel.activeSelf && EndMenuSource == null && currentLevel !=8)
        {
            source1.Pause();
            source2.Pause();

  

            EndMenuSource = gameObject.AddComponent<AudioSource>();
            EndMenuSource.clip = EndMenuClip;
            EndMenuSource.outputAudioMixerGroup = masterGroup;
            EndMenuSource.volume = volumeEndMenu;
            EndMenuSource.loop = true;
            EndMenuSource.Play();
        }
        else if (endPanel == null || !endPanel.activeSelf)
        {
            if (!source1.isPlaying) source1.UnPause();
            if (!source2.isPlaying) source2.UnPause();
        }
    }
}
