using UnityEngine;

public class LineConnectionTracker : MonoBehaviour
{
    private Node nodeA;
    private Node nodeB;
    private LineRenderer line;
    private BoxCollider2D box;
    private SpringJoint2D joint;

    private bool isBroken = false;

    public void Init(Node a, Node b, LineRenderer l, BoxCollider2D c, SpringJoint2D j)
    {
        nodeA = a;
        nodeB = b;
        line = l;
        box = c;
        joint = j;
    }

    void Update()
    {
        // if broken, do nothing
        if (isBroken) return;

        // check if join is broken
        if (joint == null)
        {
            // then detroy
            if (line != null) Destroy(line.gameObject);
            if (box != null) Destroy(box.gameObject);
            Destroy(this);
            isBroken = true;
            return;
        }

        UpdateCollider();
    }

    private void UpdateCollider()
    {
        if (nodeA == null || nodeB == null || box == null) return;

        Vector2 start = nodeA.transform.position;
        Vector2 end = nodeB.transform.position;

        // update ligne position
        if (line != null)
        {
            line.SetPosition(0, start);
            line.SetPosition(1, end);
        }

        // update ligne collider
        Vector2 midPoint = (start + end) / 2f;
        float length = Vector2.Distance(start, end);
        float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;

        box.transform.position = midPoint;
        box.transform.rotation = Quaternion.Euler(0, 0, angle);
        box.size = new Vector2(length, 0.1f);
    }
}
