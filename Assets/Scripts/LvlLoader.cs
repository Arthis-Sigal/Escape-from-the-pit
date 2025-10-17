using TMPro;
using UnityEditor;
using UnityEngine;
// Removed: using static UnityEngine.InputSystem.HID.HID;
using UnityEngine.UI;

public class LvlLoader : MonoBehaviour
{
    public GameObject LvlSelector;

    private int LvlCount = 6;

    public Sprite Star;

    [System.Obsolete]
    void Start()
    {
        GenerateLevelButtons();
    }

    [System.Obsolete]
    void GenerateLevelButtons()
    {
        // Récupère les données sauvegardées
        var saveData = SaveSystem.Load();
        int unlockedLevel = saveData.lastUnlockedLevel;

        for (int i = 1; i <= LvlCount; i++)
        {
            GameObject button = Instantiate(LvlSelector, transform);
            button.name = "Level" + i + "Button";

            // Position and dimention of button
            var rect = button.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-9 + (i - 1f) * 3f, -1f);
            // button's text
            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            text.text = "Level " + i;

            // StarCount
            int stars = 0;
            if (saveData.levelStars.TryGetValue(i, out int storedStars))
                stars = storedStars;

            //Create the star box
            for (int s = 0; s < 3; s++)
            {
                GameObject starObj = new GameObject("Star" + (s + 1));
                starObj.transform.SetParent(button.transform);
                Image starImage = starObj.AddComponent<Image>();
                starImage.sprite = Star;
                starImage.color = (s < stars) ? Color.white : Color.black;

                RectTransform starRect = starObj.GetComponent<RectTransform>();
                starRect.sizeDelta = new Vector2(2f/3f, 2f/3f);
                starRect.anchoredPosition = new Vector2(-70 + s * 70, 0);
            }

            // Disable lvl if not unlocked
            Button btn = button.GetComponent<Button>();
            int levelIndex = i; // capture
            if (i <= unlockedLevel)
            {
                btn.interactable = true;
                btn.onClick.AddListener(() => LoadLvl(levelIndex));
            }
            else
            {
                btn.interactable = false;
                text.color = Color.gray;
            }
        }
    }

    [System.Obsolete]
    public void LoadLvl(int lvl)
    {
       FindObjectOfType<SceneFader>().FadeToSceneString("level" + lvl);
    }

    [System.Obsolete]
    public void DeletSave()
    {
        SaveSystem.DeleteSave();
        //recharge la scene
        FindObjectOfType<SceneFader>().FadeToSceneString(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }


    
    
}
