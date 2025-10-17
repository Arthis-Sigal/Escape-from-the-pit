using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public bool isPaused = false;

    public GameObject pausePanel; // panel pause

    public GameObject settingsPanel; // panel paramètres

    public static GameManager Instance;
    private SaveData saveData;
    
    public GameObject NodePrefab;

    public Sprite ballonSprite;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        saveData = SaveSystem.Load();

        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (currentLevel != 8 && currentLevel !=7) //si on est pas dans l'intro si outro
        {
            //générer 25 node au centre de la scène,
            for (int i = 0; i < 25; i++)
            {
                Vector2 position = new Vector2(Random.Range(1f, 2f), 0);
                Instantiate(NodePrefab, position, Quaternion.identity);
            }

            if (currentLevel != 1) //si on pas dans le niveau 1, généré 6 ballon
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 position = new Vector2(Random.Range(-2f, -1f), 0);
                    GameObject nodeObj = Instantiate(NodePrefab, position, Quaternion.identity);
                    Node node = nodeObj.GetComponent<Node>();
                    node.isBallon = true;

                    SpriteRenderer sr = nodeObj.GetComponent<SpriteRenderer>();
                    //on met un autre sprite
                    sr.sprite = Resources.Load<Sprite>("sprites/ballon");
                }
            }

        }
        
        
    }
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    [System.Obsolete]
    public void ReturnMainMenu() //load main Menu
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel() //restart the current lvl
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    [System.Obsolete]
    public void NextLevel() //load next lvl
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        FindObjectOfType<SceneFader>().FadeToSceneInt(currentIndex + 1);
        Time.timeScale = 1f;
    }

    [System.Obsolete]
    public void GoToHeaven() //load last lvl
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        FindObjectOfType<SceneFader>().FadeToSceneString("outro");
        Time.timeScale = 1f;
    }
    public void OpenSettings() //open settings pannel
    {
        pausePanel.SetActive(false);
        // Ouvre le menu des paramètres
        settingsPanel.SetActive(true);
    }

    public void CloseSettings() //close settings pannel
    {
        pausePanel.SetActive(true);
        // Ferme le menu des paramètres
        settingsPanel.SetActive(false);
    }

    // Update is called once per frame
    public void WinLevel(int levelIndex, int stars) //Save Score
    {
        // Met à jour le score du niveau
        if (!saveData.levelStars.ContainsKey(levelIndex))
            saveData.levelStars[levelIndex] = stars;
        else
            saveData.levelStars[levelIndex] = Mathf.Max(saveData.levelStars[levelIndex], stars);

        // Débloque le niveau suivant
        saveData.lastUnlockedLevel = Mathf.Max(saveData.lastUnlockedLevel, levelIndex + 1);

        SaveSystem.Save(saveData);
    }

    public int GetStars(int levelIndex) //Get the stars from save
    {
        if (saveData.levelStars.TryGetValue(levelIndex, out int stars))
            return stars;
        return 0;
    }

    public int GetUnlockedLevel() => saveData.lastUnlockedLevel;

    private void OnApplicationQuit()
    {
        SaveSystem.Save(saveData);
    }
}
