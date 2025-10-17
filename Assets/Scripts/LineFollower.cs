using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineFollower : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (pointA == null || pointB == null)
        {
            Destroy(gameObject);
            return;
        }

        line.SetPosition(0, pointA.position);
        line.SetPosition(1, pointB.position);
    }
}
