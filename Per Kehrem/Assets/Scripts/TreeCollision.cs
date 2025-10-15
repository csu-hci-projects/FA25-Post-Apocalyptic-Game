using UnityEngine;

public class TreeCollision : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    
    void FixedUpdate()
    {
        Vector2 movement = playerMovement.GetMovement();
        if (movement == Vector2.zero) return;

        Vector2 nextPos = rb.position + movement.normalized * playerMovement.moveSpeed * Time.fixedDeltaTime;

        float avoidRadius = 1f; // radius around trees to start sliding

        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        foreach (GameObject tree in trees)
        {
            Vector2 toTree = tree.transform.position - (Vector3)nextPos;
            float distance = toTree.magnitude;

            if (distance < avoidRadius)
            {
                // Project movement vector along the tangent to the tree
                Vector2 tangent = new Vector2(-toTree.y, toTree.x).normalized;
                movement = Vector2.Dot(movement, tangent) * tangent;
            }
        }

        // Move the player along the (possibly adjusted) movement vector
        rb.MovePosition(rb.position + movement.normalized * playerMovement.moveSpeed * Time.fixedDeltaTime);
    }
}
