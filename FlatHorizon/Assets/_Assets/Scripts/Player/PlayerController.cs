using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;

    [Header("Salto")]
    public float jumpForce = 10f;
    public float jumpHoldTime = 0.2f;
    public float jumpCutMultiplier = 0.5f;

    [Header("Verificación de suelo")]
    public LayerMask groundLayer;
    public Transform groundCheck;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float jumpTimeCounter;
    private bool isJumpingHeld;
    private bool isDead = false;

    private enum MovementState { Run, Jump, Death }
    private MovementState state;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Crear groundCheck si no está asignado
        if (groundCheck == null)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.parent = transform;
            gc.transform.localPosition = new Vector3(0, -0.6f, 0);
            groundCheck = gc.transform;
        }
    }

    void Update()
    {
        if (isDead) return;

        // Verificar si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Iniciar salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTimeCounter = jumpHoldTime;
            isJumpingHeld = true;
        }

        // Mantener salto si se sigue presionando
        if (Input.GetButton("Jump") && isJumpingHeld)
        {
            if (jumpTimeCounter > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumpingHeld = false;
            }
        }

        // Cortar salto si se suelta antes
        if (Input.GetButtonUp("Jump"))
        {
            isJumpingHeld = false;

            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
        }

        UpdateAnimationState();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        // Movimiento horizontal constante (autorunner)
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    void UpdateAnimationState()
    {
        if (isDead)
        {
            state = MovementState.Death;
        }
        else if (!isGrounded)
        {
            state = MovementState.Jump;
        }
        else
        {
            state = MovementState.Run;
        }

        animator.SetInteger("state", (int)state);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GameOverZone"))
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        animator.SetInteger("state", (int)MovementState.Death);
        AudioManager.Instance?.PlayDeathSound();
        GameManager.Instance?.GameOver();
    }

    void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}
