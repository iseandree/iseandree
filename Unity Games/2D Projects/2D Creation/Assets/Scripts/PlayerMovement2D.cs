using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float jumpCooldown = 0.50f;
    private float lastJumpTime;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody2D rb;
    
    private LayerMask groundLayer;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        groundLayer = LayerMask.GetMask("Ground");
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        lastJumpTime = -jumpCooldown;
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void FixedUpdate()
    {
       isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.75f, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.black);
        //rb.linearVelocityX = moveAction.ReadValue<Vector2>().x * moveSpeed;
        rb.AddForce(Vector2.right * moveAction.ReadValue<Vector2>().x * moveSpeed, ForceMode2D.Force);
        if (jumpAction.IsPressed() && isGrounded && Time.time >= lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
        }
    }
}
