using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(Rigidbody2D))]
public class Node : MonoBehaviour
{
    [Header("Node Settings")]
    public Rigidbody2D rb;
    public bool isActive = false;
    public List<Node> connectedNodes = new List<Node>();
    public List<SpringJoint2D> joints = new List<SpringJoint2D>();
    public List<LineRenderer> lines = new List<LineRenderer>();
    public bool isBallon = false;

    public bool isAnchor = false;

    [Header("Spring Settings")]
    public float maxDistance = 0f;
    public float springFrequency = 0f; // frequency
    public float springDamping = 0f; // damping
    public float breakForce = 0f; // max force before break

    public float ballonForce = 100f; //ballon force up

    [Header("Line Settings")]
    public Material lineMaterial;
    public float lineWidth = 0.05f;

    [Header("Physics Settings")]
    public string bridgeLayer = "Bridge"; // le nom du layer à utiliser

    [Header("SoundDesign")]
    private AudioClip connectSound = null;
    [Range(0f, 1f)]
    public float volumeConnectSound = 0.5f;
    public AudioMixerGroup masterGroup;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 1f;
        rb.gravityScale = 1f;

        springFrequency = 0f; // rigidité
        springDamping = 0.4f; // amortissement
        breakForce = 120f;

        connectSound = Resources.Load<AudioClip>("sound/WoodBarSound");
        connectSound.LoadAudioData();
    }

    private void FixedUpdate()
    {
        for (int i = connectedNodes.Count - 1; i >= 0; i--)
        {
            if (i >= joints.Count || i >= lines.Count || connectedNodes[i] == null || joints[i] == null || lines[i] == null)
            {
                if (i < connectedNodes.Count) connectedNodes.RemoveAt(i);
                if (i < joints.Count) joints.RemoveAt(i);
                if (i < lines.Count) lines.RemoveAt(i);
                continue;
            }
        }


        for (int i = 0; i < connectedNodes.Count; i++)
        {
            Node other = connectedNodes[i];
            LineRenderer line = lines[i];

            line.SetPosition(0, transform.position);
            line.SetPosition(1, other.transform.position);

            float forceRatio = joints[i].reactionForce.magnitude / breakForce;
            line.startColor = line.endColor = Color.Lerp(Color.white, Color.darkRed, forceRatio);

            LineConnectionTracker tracker = line.GetComponent<LineConnectionTracker>();
            
        }

        if (isActive && isBallon && connectedNodes.Count != 0)
        {
            //levitate streng for ballon
            rb.AddForce(Vector2.up * ballonForce);
        }

}

    [System.Obsolete]
    public void TryConnect(Node other)
{
    if (connectedNodes.Contains(other) || !isActive || !other.isActive) return;

    //create physical join
    SpringJoint2D joint = gameObject.AddComponent<SpringJoint2D>();
    joint.connectedBody = other.rb;
    joint.autoConfigureDistance = false;
    joint.distance = Vector2.Distance(transform.position, other.transform.position);
    joint.dampingRatio = 10f;
    joint.frequency = 5f;
    joint.breakForce = breakForce;

    joints.Add(joint);
    connectedNodes.Add(other);
        other.connectedNodes.Add(this);
    
    AudioSource source1 = gameObject.AddComponent<AudioSource>();
    source1.clip = connectSound;
    source1.outputAudioMixerGroup = masterGroup;
    source1.volume = volumeConnectSound;
    source1.loop = false;
    source1.Play();

    // create line renderer
    GameObject lineObj = new GameObject($"Line_{name}_to_{other.name}");
    LineRenderer line = lineObj.AddComponent<LineRenderer>();
    line.positionCount = 2;
    line.startWidth = lineWidth;
    line.endWidth = lineWidth;
    line.material = lineMaterial;
    line.useWorldSpace = true;
    line.startColor = line.endColor = Color.white;
    line.sortingOrder = 0;
    lines.Add(line);

    // create collider
    GameObject boxObj = new GameObject($"Collider_{name}_to_{other.name}");
    BoxCollider2D box = boxObj.AddComponent<BoxCollider2D>();
    box.usedByComposite = false;
    boxObj.transform.parent = this.transform.parent;
        // define layer to bridge
        boxObj.layer = LayerMask.NameToLayer(bridgeLayer);
    
    // add rigid body
    Rigidbody2D boxRb = boxObj.AddComponent<Rigidbody2D>();
    boxRb.bodyType = RigidbodyType2D.Kinematic;
    boxRb.simulated = true;
    boxRb.gravityScale = 1f;
    boxRb.mass = 0.1f;

    boxObj.transform.parent = this.transform.parent;


    // Initial postion
        Vector2 start = transform.position;
    Vector2 end = other.transform.position;
    Vector2 midPoint = (start + end) / 2f;
    float length = Vector2.Distance(start, end);
    float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;

    boxObj.transform.position = midPoint;
    boxObj.transform.rotation = Quaternion.Euler(0, 0, angle);
    box.size = new Vector2(length, 0.1f); // épaisseur du pont

    // add the line tracker
    LineConnectionTracker tracker = boxObj.AddComponent<LineConnectionTracker>();
    tracker.Init(this, other, line, box, joint);

}



    public void AddConnection(Node other, SpringJoint2D spring, LineRenderer line)
    {
        if (!connectedNodes.Contains(other))
        {
            connectedNodes.Add(other);
            joints.Add(spring);
            lines.Add(line);
        }
    }
}
