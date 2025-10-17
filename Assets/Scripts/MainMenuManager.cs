using UnityEngine;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{

    public GameObject settingsPanel; // panel paramètres
    public GameObject mainMenuPanel; // panel menu principal
    public GameObject lvlSelectPanel; // panel sélection niveau
    public GameObject SkinSelectPanel;
    public AudioMixerGroup masterGroup;

    public AudioClip MenuAmbientSound;
        [Range(0f, 1f)]
    public float volumeMenuAmbient = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        MenuAmbientSound.LoadAudioData();
        AudioSource source1 = gameObject.AddComponent<AudioSource>();
        source1.clip = MenuAmbientSound;
        source1.outputAudioMixerGroup = masterGroup;
        source1.volume = volumeMenuAmbient;
        source1.loop = true;
        source1.Play();
    }

    [System.Obsolete]
    public void PlayGame()
    {
        Debug.Log("ButtonPress");
        // Charge la scène de jeu (assurez-vous que la scène de jeu est ajoutée dans les paramètres de build)
        FindObjectOfType<SceneFader>().FadeToSceneString("Intro");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void OpenLevelSelect()
    {
        lvlSelectPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    public void CloseLevelSelect()
    {
        lvlSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void OpenSkinPanel()
    {
        SkinSelectPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    public void CloseSkinPanel()
    {
        SkinSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
