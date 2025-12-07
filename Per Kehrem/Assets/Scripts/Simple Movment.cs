using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    // Animator is enabled/disabled directly now
    public Animator animator;
    [Tooltip("Movement magnitude threshold to consider walking.")]
    public float walkThreshold = 0.01f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();

        // start disabled if not moving
        if (animator != null)
            animator.enabled = false;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool isWalking = movement.sqrMagnitude > walkThreshold * walkThreshold;

        if (animator != null)
        {
            // only change enabled state when it differs to avoid overhead
            if (isWalking && !animator.enabled)
                animator.enabled = true;
            else if (!isWalking && animator.enabled)
                animator.enabled = false;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}