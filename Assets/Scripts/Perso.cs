using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Perso : MonoBehaviour
{

    public bool canMove = false;
    public bool isDead = false;
    public float moveSpeed = 3f;

    public TMP_Text NbStarsTxt;

    public GameObject endPanel; 
    public GameObject deathPanel; 

    private Animator animator;

    public int TwoStarsConditionNode;
    public int ThreeStarsConditionNode;
    public int TwoStarsConditionBallon;
    public int ThreeStarsConditionBallon;

    public GameObject starBox;
    public Sprite starSprite;  

    [Range(0, 3)] public int currentStars = 1;  
    public float starSpacing = 100f;     
    public Vector2 starSize = new Vector2(100, 100); 

    public AudioClip deathSound;

    public AudioMixerGroup masterGroup;


    void Start()
    {
        animator = GetComponent<Animator>();
        endPanel.SetActive(false);
        deathPanel.SetActive(false);

          deathSound.LoadAudioData();

    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
        }
        if (!canMove && !isDead)
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void Play()
    {
        canMove = true;
    }

    [Obsolete]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            Debug.Log("Gagné");
            canMove = false;

            endPanel.SetActive(true);

            Node[] allNodes = FindObjectsOfType<Node>();
            int nodeCount = allNodes.Count(n => n.isActive);
            int ballonCount = allNodes.Count(n => n.isActive && n.isBallon);
            int anchorCount = allNodes.Count(n => n.isAnchor);
            int basicNodeCount = nodeCount - ballonCount - anchorCount;

            Debug.Log("Total Nodes: " + nodeCount);
            Debug.Log("Total basic node: " + (nodeCount - ballonCount - anchorCount));
            Debug.Log("Total Ballons: " + ballonCount);
            Debug.Log("Total Anchor: " + anchorCount);

            //2 stars condition
            if ((basicNodeCount <= TwoStarsConditionNode) && (ballonCount <= TwoStarsConditionBallon))
            {
                currentStars = 2;
            }

            //3 stars condition
            if ((basicNodeCount <= ThreeStarsConditionNode) && (ballonCount <= ThreeStarsConditionBallon))
            {
                currentStars = 3;
            }

            //display star win
             for (int s = 0; s < 3; s++)
        {
            GameObject starObj = new GameObject("Star" + (s + 1));
            starObj.transform.SetParent(starBox.transform);
            starObj.transform.localScale = Vector3.one;

            Image starImage = starObj.AddComponent<Image>();
            starImage.sprite = starSprite;
            starImage.color = (s < currentStars) ? Color.white : Color.black; //don't earned star in black
            RectTransform starRect = starObj.GetComponent<RectTransform>();
            starRect.sizeDelta = starSize;
            starRect.anchorMin = new Vector2(0.5f, 0.5f);
            starRect.anchorMax = new Vector2(0.5f, 0.5f);
            starRect.pivot = new Vector2(0.5f, 0.5f);
            starRect.anchoredPosition = new Vector2(-starSpacing + s * starSpacing, 0);
        }

            //get star save
            var saveData = SaveSystem.Load();
            int previousStars = 0;
            if (saveData.levelStars.TryGetValue(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, out int storedStars))
                previousStars = storedStars;

            //keep hight score
            currentStars = Math.Max(currentStars, previousStars);

            //when player finish a lvl, 
            int currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            GameManager.Instance.WinLevel(currentLevel, currentStars);
        }

        if (other.CompareTag("DeathZone"))
        {
            Debug.Log("Perdu");
            canMove = false;
            isDead = true;
            AudioSource source1 = gameObject.AddComponent<AudioSource>();
            source1.clip = deathSound;
            source1.outputAudioMixerGroup = masterGroup;
            source1.volume = 1f;
            source1.Play();
            animator.SetBool("isDead", true);
            deathPanel.SetActive(true);
        }
    }
}


