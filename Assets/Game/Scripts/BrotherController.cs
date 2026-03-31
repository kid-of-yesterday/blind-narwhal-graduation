using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BrotherController : MonoBehaviour
{
    [Header("Follow")]
    public float followDistance = 1.2f;
    public float followSpeed = 4f;
    public float catchupSpeed = 6f;
    public float stopDistance = 0.15f;

    [Header("Hand Hold")]
    public float handHoldOffset = 0.8f;

    private PlayerController player;
    private Rigidbody2D rb;
    private Collider2D brotherCollider;
    private Collider2D playerCollider;

    private bool isHandHolding;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        brotherCollider = GetComponent<Collider2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    public void AssignPlayer(PlayerController playerController)
    {
        player = playerController;

        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();

            if (playerCollider != null && brotherCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, brotherCollider, true);
            }
        }
    }

    public void SetHandHolding(bool holding)
    {
        isHandHolding = holding;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        if (isHandHolding)
            DoHandHold();
        else
            DoFollow();
    }

    private void DoFollow()
    {
        if (!player.IsActuallyMoving)
            return;

        float targetX = player.transform.position.x - (player.FacingDirection * followDistance);
        float currentX = rb.position.x;
        float diff = targetX - currentX;
        float absDiff = Mathf.Abs(diff);

        if (absDiff <= stopDistance)
            return;

        float speed = absDiff > 2f ? catchupSpeed : followSpeed;
        float step = Mathf.Sign(diff) * speed * Time.fixedDeltaTime;
        float nextX = currentX + step;

        if (Mathf.Abs(targetX - nextX) < Mathf.Abs(step))
            nextX = targetX;

        Vector2 nextPos = new Vector2(nextX, rb.position.y);
        rb.MovePosition(nextPos);
    }

    private void DoHandHold()
    {
        float side = -player.FacingDirection;

        Vector2 targetPos = new Vector2(
            player.transform.position.x + (handHoldOffset * side),
            player.transform.position.y
        );

        rb.MovePosition(targetPos);
    }
}