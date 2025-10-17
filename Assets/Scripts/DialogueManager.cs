using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Settings")]
    public float typingSpeed = 0.03f;

    private Queue<string> sentences = new Queue<string>();
    private bool isTyping = false;
    private string currentSentence = "";
    public bool dialogueActive = false;


    [Header("Réglages de l'effet de respiration")]
    public float speed = 2f;         // Vitesse de la respiration
    public float minAlpha = 0.4f;    // Opacité minimale
    public float maxAlpha = 1f;      // Opacité maximale
    private Color baseColor;
    public SpriteRenderer Character;
    private bool IsEnd = false;

    public GameObject EndPanel;

    private int currentLevel;

    void Start()
    {
        dialogueBox.SetActive(true);
        //on récupére le niveau en cours
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        Character = GetComponent<SpriteRenderer>();
        baseColor = Character.color;

        if (EndPanel != null) EndPanel.SetActive(false);
 
    }

    public void StartDialogue(List<string> dialogueLines)
    {
        dialogueActive = true;
        dialogueBox.SetActive(true);

        sentences.Clear();
        foreach (string sentence in dialogueLines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    void Update()
    {

        if(currentLevel == 8 && !IsEnd)
        {
             // Calcul sinusoidal pour lisser la transition
            float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f; // → 0 à 1
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

            Color newColor = baseColor;
            newColor.a = alpha;
            Character.color = newColor;

            
            
        }

        if (!dialogueActive) return;

        // Clique gauche = passer ou aller à la suivante
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            if (isTyping)
            {
                // Termine immédiatement la phrase en cours
                StopAllCoroutines();
                dialogueText.text = currentSentence;
                isTyping = false;
            }
            else
            {
                DisplayNextSentence();
            }
        }
        
        if (IsEnd)
        {
            Color color = Character.color;
            color.a = color.a - 0.01f;
            Character.color = color;
        }
    }

    public void DisplayNextSentence()
    {

        if (currentLevel == 8 && sentences.Count <= 4) EndGameScript(); 

        if (sentences.Count == 0)
        {
            if(currentLevel == 7)
            {

                GameObject Character = GameObject.Find("Character");
                introScript introScript = Character.GetComponent<introScript>();
                introScript.IsDialogFinish = true;
                EndDialogue();
                return;
            }
            EndDialogue();
            if (EndPanel != null) EndPanel.SetActive(true);
            return;
        }

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        dialogueBox.SetActive(false);
    }

    private void  EndGameScript()
    {
        IsEnd = true;
    }
}
