using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float sneakSpeed = 2.5f;
    public float jumpForce = 12f;
    public float handHoldSpeedMultiplier = 0.65f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Brother")]
    public BrotherController brother;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private bool isSneaking;
    private bool isHoldingBrother;
    private int facingDirection = 1;

    private float slowTimer;
    private float currentSlowMultiplier = 1f;

    public bool IsSneaking => isSneaking;
    public bool IsHoldingBrother => isHoldingBrother;
    public int FacingDirection => facingDirection;
    public float MoveInput => moveInput;
    public bool IsActuallyMoving => Mathf.Abs(moveInput) > 0.01f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (brother != null)
        {
            brother.AssignPlayer(this);
        }
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isSneaking = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.C);

        if (moveInput > 0.01f)
            facingDirection = 1;
        else if (moveInput < -0.01f)
            facingDirection = -1;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleHandHold();
        }

        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;

            if (slowTimer <= 0f)
            {
                currentSlowMultiplier = 1f;
            }
        }
    }

    private void FixedUpdate()
    {
        float baseSpeed = isSneaking ? sneakSpeed : moveSpeed;

        if (isHoldingBrother)
            baseSpeed *= handHoldSpeedMultiplier;

        float finalSpeed = baseSpeed * currentSlowMultiplier;

        rb.linearVelocity = new Vector2(moveInput * finalSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 2f * Time.fixedDeltaTime;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void ToggleHandHold()
    {
        isHoldingBrother = !isHoldingBrother;

        if (brother != null)
        {
            brother.SetHandHolding(isHoldingBrother);
        }
    }

    public void ApplySlow(float multiplier, float duration)
    {
        currentSlowMultiplier = multiplier;
        slowTimer = duration;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}