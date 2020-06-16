using UnityEngine;

/// <summary>
/// Handles the physics that should be applied to the character.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerCharacterController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D boxCollider;


    [Header("Moving")]

        [Tooltip("Move speed of the character")]
        [SerializeField] private float speed = 10f;

        [Tooltip("Jump impulse velocity")]
        [SerializeField] private float jumpVelocity = 8f;


    private bool grounded;
    private bool wasGrounded;
    public bool landed { get; private set; }

    [Header("Ground check")]

        [Tooltip("Distance to the ground in which the character is considered as grounded")]
        [SerializeField] private float bottomBoxcastDist = 1f;

        [Tooltip("Layers that are considered as ground")]
        [SerializeField] private LayerMask whatIsGround;


    private bool falling;

    [Header("Falling")]

        [Tooltip("Fall speed smoothing")]
        [SerializeField] private float fallSmoothing = 0.5f;

        [Tooltip("Amount by which the gravity is multiplied on the player when falling")]
        [SerializeField] private float fallMultiplier = 2f;


    private Vector2 refVelocity;
    private bool stoppedJumping;

    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        grounded = IsGrounded();

        if (!wasGrounded && grounded)
            landed = true;
        else
            landed = false;

        if ((rb.velocity.y < 0 || stoppedJumping) && !grounded)
        {
            falling = true;
        }
        else if(grounded)
        {
            falling = false;
        }

        if (stoppedJumping)
            stoppedJumping = false;

        wasGrounded = grounded;
    }

    private void FixedUpdate()
    {
        if(falling)
        {
            Fall();
        }
    }

    /// <summary>
    /// Handles character movement.
    /// </summary>
    /// <param name="direction">Moves left or right depending on the sign.</param>
    /// <param name="jump">Jumps if set to true.</param>
    public void Move(float direction, bool jump)
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if(jump && grounded)
        {
            Jump();
        }

        if((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
        {
            Flip();
        }
    }

    //Handles the character jump.
    private void Jump()
    {
        grounded = false;

        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }

    //Gives the character's falls an accelerated smoothed effect.
    private void Fall()
    {
        Vector2 targetVelocity = new Vector2(rb.velocity.x, Physics2D.gravity.y * fallMultiplier);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref refVelocity, fallSmoothing);

        //rb.velocity = new Vector2(rb.velocity.x, Physics2D.gravity.y * fallMultipplier);
    }

    //Flips the character when turning left or right.
    private void Flip()
    {
        rb.transform.localScale = new Vector3(rb.transform.localScale.x * -1f, rb.transform.localScale.y, rb.transform.localScale.z);
        isFacingRight = !isFacingRight;
    }

    /// <summary>
    /// Checks if the character is grounded.
    /// </summary>
    /// <returns><c>true</c> if grounded, <c>false</c> if not.</returns>
    public bool IsGrounded()
    {
        //Appearently the overlapbox method is slightly better perfomance wise
        //RaycastHit2D hit = Physics2D.BoxCast(rb.position, boxCollider.bounds.size, 0f, Vector2.down, bottomBoxcastDist, whatIsGround);

        Collider2D collider = Physics2D.OverlapBox(rb.position + Vector2.down * boxCollider.bounds.extents.y, new Vector2(boxCollider.bounds.extents.x, bottomBoxcastDist * 2f), 0f, whatIsGround);

        Color rayColor;

        if (collider != null)
            rayColor = Color.green;
        else
            rayColor = Color.red;

        Debug.DrawRay(new Vector2(rb.position.x - boxCollider.bounds.extents.x, rb.position.y - boxCollider.bounds.extents.y), Vector2.down * bottomBoxcastDist, rayColor);
        Debug.DrawRay(new Vector2(rb.position.x + boxCollider.bounds.extents.x, rb.position.y - boxCollider.bounds.extents.y), Vector2.down * bottomBoxcastDist, rayColor);
        Debug.DrawRay(new Vector2(rb.position.x + boxCollider.bounds.extents.x, rb.position.y - boxCollider.bounds.extents.y - bottomBoxcastDist), Vector2.left * boxCollider.bounds.size.x, rayColor);

        return collider != null;
    }

    /// <summary>
    /// Checks if the player is going upwards without being grounded.
    /// </summary>
    public bool IsJumping()
    {
        return (!IsGrounded() && rb.velocity.y > 0);
    }

    /// <summary>
    /// Checks if the player is going downwards without being grounded.
    /// </summary>
    public bool IsFalling()
    {
        return (!IsGrounded() && rb.velocity.y < 0);
    }

    /// <summary>
    /// Stops the jump in the middle if <c>input</c> is true.
    /// </summary>
    public void StopJumping(bool input)
    {
        stoppedJumping = input;
    }
}
