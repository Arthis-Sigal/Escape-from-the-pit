using TMPro;
using UnityEngine;

public class DialogueSkinScript : MonoBehaviour
{

    public GameObject BorisFace;
    public GameObject BeatriceFace;
    public TMP_Text Name;

    private int skinChosen;

    //1 = boris / 2 = beatrice
    void Start()
    {
        skinChosen = PlayerPrefs.GetInt("SkinChosen", 1);

        if (skinChosen == 1)
        {
            BeatriceFace.SetActive(false);
            if (Name != null) Name.text = "Boris";
        }
        if (skinChosen == 2)
        {
            BorisFace.SetActive(false);
            if (Name != null) Name.text = "Beatrice";
   
        }
    }

}
