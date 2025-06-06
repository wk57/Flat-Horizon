using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;

    [Header("Salto")]
    public float jumpForce = 10f;
    public float jumpHoldTime = 0.2f;         // Duraci�n m�xima que se puede mantener el salto
    public float jumpCutMultiplier = 0.5f;    // Cu�nto se reduce la fuerza si se suelta antes

    [Header("Verificacion de suelo")]
    public LayerMask groundLayer;
    public Transform groundCheck;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float jumpTimeCounter;
    private bool isJumpingHeld;

    private enum MovementState { running, jumping}
    private MovementState state = MovementState.running;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Crear groundCheck si no est� asignado
        if (groundCheck == null)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.parent = transform;
            gc.transform.localPosition = new Vector3(0, -0.6f, 0); // Ajust� seg�n tama�o del sprite
            groundCheck = gc.transform;
        }
    }

    void FixedUpdate()
    {
        // Movimiento horizontal constante (autorunner)
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);

        // Verificar si est� en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Actualizar animaciones
        animator.SetBool("IsJumping", !isGrounded);
    }

    void Update()
    {
        // Iniciar salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTimeCounter = jumpHoldTime;
            isJumpingHeld = true;
            animator.SetTrigger("JumpTrigger");
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            animator.SetTrigger("Death");
            moveSpeed = 0f;
            AudioManager.Instance.PlayDeathSound();
            rb.linearVelocity = Vector2.zero;             // Detener velocidad
            rb.angularVelocity = 0f;                // Detener rotación (si aplica)
            rb.bodyType = RigidbodyType2D.Kinematic; // Hacerlo inmune a la física

            GameManager.Instance.GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GameOverZone"))
        {
            Debug.Log("vacio");
            moveSpeed = 0f;
            AudioManager.Instance.PlayDeathSound();
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;

            GameManager.Instance.GameOver();
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}
