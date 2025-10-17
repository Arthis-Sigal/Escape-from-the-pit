using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; 
    public GameObject settingsPanel; 
    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false); 
        settingsPanel.SetActive(false); 
    }

    void Update()
    {
        // Touche Echap pour pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quitter le jeu");
        Application.Quit();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}
