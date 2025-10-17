using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    [Header("Personnages")]
    public GameObject Boris;
    public GameObject Beatrice;

    public GameObject BeatriceSelector;
    public GameObject BorisSelector;
    private int skinChosen = 1;

    void Start()
    {
        //load saved skin
        skinChosen = PlayerPrefs.GetInt("SkinChosen", 1);

        //apply choosen skin
        ApplySkin();

        if (BorisSelector != null && BeatriceSelector != null)
        {
            if (skinChosen == 1)
            {
                BeatriceSelector.GetComponent<SpriteRenderer>().color = Color.black;
                BorisSelector.GetComponent<SpriteRenderer>().color = Color.white;
            }
            if (skinChosen == 2)
            {
                BorisSelector.GetComponent<SpriteRenderer>().color = Color.black;
                BeatriceSelector.GetComponent<SpriteRenderer>().color = Color.white;
            }
  
        }
    }

    public void BorisIsChosen()
    {
        skinChosen = 1;
        BeatriceSelector.GetComponent<SpriteRenderer>().color = Color.black;
        BorisSelector.GetComponent<SpriteRenderer>().color = Color.white;
        SaveSkinChoice();
        ApplySkin();
    }

    public void BeatriceIsChosen()
    {
        skinChosen = 2;
        BorisSelector.GetComponent<SpriteRenderer>().color = Color.black;
        BeatriceSelector.GetComponent<SpriteRenderer>().color = Color.white;
        SaveSkinChoice();
        ApplySkin();
    }

    private void ApplySkin()
    {
        if (Boris != null || Beatrice != null)
        {
            Boris.SetActive(skinChosen == 1);
            Beatrice.SetActive(skinChosen == 2);
        }

     
  

    }

    private void SaveSkinChoice()
    {
        // save skin choosen in plyerpreds
        PlayerPrefs.SetInt("SkinChosen", skinChosen);
        PlayerPrefs.Save();
    }
}
