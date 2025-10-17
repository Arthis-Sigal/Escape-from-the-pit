using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NodeDragger : MonoBehaviour
{
    private Camera cam;
    private Node selectedNode;
    private Vector3 offset;

    [Header("Paramètres du drag")]
    public float connectRange = 2f;

    [Header("Temp line settings")]
    public Material tempLineMaterial;
    public float tempLineWidth = 0.05f;


    // Liste des lignes temporaires
    private List<LineRenderer> tempLines = new List<LineRenderer>();
    private List<Node> tempTargets = new List<Node>();

    private bool IntroOnGoing = true;

      private int currentLevel;

    private void Start()
    {
        cam = Camera.main;

        currentLevel = SceneManager.GetActiveScene().buildIndex;


        
    }

    [System.Obsolete]
    private void Update()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // --- PICK ---
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);
            if (hit != null)
            {
                Node node = hit.GetComponent<Node>();
                // règle : on ne peut pas attraper un node déjà connecté
                if (node != null && node.connectedNodes.Count == 0)
                {
                    selectedNode = node;
                    selectedNode.isActive = true;
                    offset = selectedNode.transform.position - (Vector3)mouseWorld;
                }
            }
        }

        if (IntroOnGoing && currentLevel != 3 && currentLevel !=5)
        {
            //get the variable IsDialogFinish
            GameObject Dialogue = GameObject.Find("DialogueManager");
            DialogueManager introScript = Dialogue.GetComponent<DialogueManager>();
            IntroOnGoing = introScript.dialogueActive;
            Debug.Log("entré");
        }

        if (currentLevel == 3 || currentLevel == 5) IntroOnGoing = false;


        // --- DRAG ---
        if (Mouse.current.leftButton.isPressed && selectedNode != null && selectedNode.isAnchor == false && !IntroOnGoing)
        {


            selectedNode.rb.MovePosition(mouseWorld + (Vector2)offset);

            // create temp line
            UpdateTempLines();
        }

        // --- RELEASE ---
        if (Mouse.current.leftButton.wasReleasedThisFrame && selectedNode != null)
        {
       

            // connect all close node
            Collider2D[] nearby = Physics2D.OverlapCircleAll(selectedNode.transform.position, connectRange);
            foreach (var c in nearby)
            {
                Node other = c.GetComponent<Node>();
                if (other != null && other != selectedNode)
                    selectedNode.TryConnect(other);
            }

   
            ClearTempLines();

            selectedNode = null;
        }
    }


    private void UpdateTempLines()
    {
   
        ClearTempLines();

        // look for close node
        Collider2D[] hits = Physics2D.OverlapCircleAll(selectedNode.transform.position, connectRange);
        foreach (var c in hits)
        {
            Node other = c.GetComponent<Node>();
            if (other == null || other == selectedNode) continue;
            if (!selectedNode.isActive && !other.isActive) continue;
            if (selectedNode.connectedNodes.Contains(other)) continue;

            // create temps line
            GameObject lineObj = new GameObject("TempLine_" + selectedNode.name + "_to_" + other.name);
            LineRenderer line = lineObj.AddComponent<LineRenderer>();
            line.material = tempLineMaterial;
            line.startWidth = tempLineWidth;
            line.endWidth = tempLineWidth;
            line.positionCount = 2;
            line.useWorldSpace = true;


            line.SetPosition(1, selectedNode.transform.position);
            line.SetPosition(0, other.transform.position);

            tempLines.Add(line);
            tempTargets.Add(other);
        }
    }


    private void ClearTempLines()
    {
        foreach (var line in tempLines)
        {
            if (line != null) Destroy(line.gameObject);
        }
        tempLines.Clear();
        tempTargets.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        if (selectedNode != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(selectedNode.transform.position, connectRange);
        }
    }
}
