using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2D : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isNear;
    [SerializeField] private float jumpCooldown = 0.50f;
    [SerializeField] private float circleCastRadius = 0.75f;
    
    // Private Variables
    private float lastJumpTime;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private Rigidbody2D rb;
    private LayerMask groundLayer;
    private LayerMask borderLayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        groundLayer = LayerMask.GetMask("Ground");
        borderLayer = LayerMask.GetMask("Border");
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        interactAction = playerInput.actions.FindAction("Interact");
        lastJumpTime = -jumpCooldown;
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void FixedUpdate()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.75f, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.black);


        // Check if the player is near a collectible object
        isNear = Physics2D.CircleCast(transform.position, circleCastRadius, Vector2.zero, 1f, borderLayer);
        
        if (isNear && interactAction.WasPressedThisFrame())
        {
            Collect();
        }

        //rb.linearVelocityX = moveAction.ReadValue<Vector2>().x * moveSpeed;  Old way to move the player horizontally

        rb.AddForce(Vector2.right * moveAction.ReadValue<Vector2>().x * moveSpeed, ForceMode2D.Force);
        if (jumpAction.IsPressed() && isGrounded && Time.time >= lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
        }


    }

    private void Collect()
    {
        
        Debug.Log("Interact Pressed within range");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, circleCastRadius);
    }
}
