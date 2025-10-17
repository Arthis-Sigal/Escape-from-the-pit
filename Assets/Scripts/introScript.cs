using UnityEngine;
using UnityEngine.SceneManagement;

public class introScript : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Animator animator;

    public bool IsDialogFinish = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    [System.Obsolete]
    void Update()
    {
        if (transform.position.x < 0)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
            return;
        }

        if (transform.position.x > 10)
        {
            FindObjectOfType<SceneFader>().FadeToSceneInt(1);
            return;
        }
        if (IsDialogFinish)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
            return;
        }
        Debug.Log("pas bouger void");
        // veocity to 0
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        animator.SetBool("isWalking", false);

    }
}
