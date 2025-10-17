using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LevelIntro : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void Start()
    {
        List<string> lines = null;

        //on récupére le niveau en cours
        int currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        if (currentLevel == 1)
        {
            lines = new List<string>()
            {
                "Hmm... Je dois passer de l'autre côté de cette crevasse.",
                "Je dois absolument quitter les enfers....",
                "Ces boules de l'enfer me seront utiles pour traverser.",
            };

        }

        if (currentLevel == 2)
        {
            lines = new List<string>()
            {
                "Cette crevasse est bien plus grande que la précédente...",
                "Ces boules d'éther aideront à soutenir le pont.",
                "Vite, cet endroit est dangereux.",
            };

        }

        if (currentLevel == 4)
        {
            lines = new List<string>()
            {
                "Cette chaleur est insoutenable...",
            };

        }

        if (currentLevel == 6)
        {
            lines = new List<string>()
            {
                "Les portes du Paradis !!! enfin...",
                "...",
                "Mon dieu... Cette crevasse est immense",
            };

        }

        if (currentLevel == 8) //outro
        {
            lines = new List<string>()
            {
                "...",
                "...",
                "Cette luimère...",
                "Elle m'éblouit...",
                "Suis-je au paradis ? Ai-je reussi ?",
                "...",
                "Cette lumière... Elle est de plus en plus forte..",
                "Je me sens bien...",
                "Enfin",
                "...",
                "AHHHHHHHHHHHHHHH",
                "...",
                "...",
                "...",
            };

        }
        
        if (currentLevel == 7) //intro
        {
            lines = new List<string>()
            {
                "Piouf",
                "J'ai reussi à echaper au regard des démons",
                "Vite il faut trouver cette porte",
            };

        }


        dialogueManager.StartDialogue(lines);
    }
}
