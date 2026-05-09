using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    // Serialized fields - Movement
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float acceleration = 6.0f;
    [SerializeField] private float deceleration = 6.0f;
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private float jumpCooldown = 0.50f;


    // Serialized fields - Debug checks
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isNear;
    [SerializeField] private float circleCastRadius = 0.75f;
    [SerializeField] private float rayCastLength = 2f;

    // Private Variables
    private float lastJumpTime;
    private float runSpeed;
    private float walkSpeed;
    private float moveInput;
    private bool moveInputPresent;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction interactAction;
    private Rigidbody2D rb;
    private LayerMask groundLayer;
    private LayerMask borderLayer;
    private GameObject interactable;
    private bool isFacingLeft = false;
    private bool jumpRequest = false;
    private Animator animator;

    // Public Variables
    public List<GameObject> inventory;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        groundLayer = LayerMask.GetMask("Ground");
        borderLayer = LayerMask.GetMask("Border");
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        runAction = playerInput.actions.FindAction("Run");
        interactAction = playerInput.actions.FindAction("Interact");
        lastJumpTime = -jumpCooldown;
        walkSpeed = moveSpeed;
        runSpeed = moveSpeed * speedMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        // Get horizontal input
        moveInput = moveAction.ReadValue<Vector2>().x;

        // Update 'isWalking' based on whether there is any input (regardless of direction)
        moveInputPresent = Mathf.Abs(moveInput) > 0.1f;
        isRunning = runAction.IsPressed();
        HorizontalMovementAnimations();

        // Handle flipping separately
        FaceInputDirection(moveInput);

        // Allow the player to jump but prevent the player from spamming jump
        if (jumpAction.WasPerformedThisFrame() && isGrounded && Time.time >= lastJumpTime + jumpCooldown)
        {
            jumpRequest = true;
        }

        if(jumpAction.WasReleasedThisFrame() && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // Check if the player is near a collectible object
        if (DetectInteractable() && interactAction.WasPressedThisFrame())
        {
            CollectItem(interactable);
        }
    }

    private void FixedUpdate()
    {
        // Check if the player is grounded
        CheckIsGrounded();

        //Horizontal movement
        float currentMaxSpeed = isRunning ? runSpeed : walkSpeed;
        float targetSpeed = moveInput * currentMaxSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = speedDiff * accelRate;
        rb.AddForce(Vector2.right * movement, ForceMode2D.Force);

        if(jumpRequest)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequest = false;
        }
    }

    private bool DetectInteractable()
    {
        // Get a list of all possible colliders within the range of the player
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCastRadius);

        // Check if there are any colliders within the range of the player. If not close the method
        if (hits.Length == 0)
        {
            interactable = null;
            Debug.Log("No collisions detected");
            return false;
        }

        // Variables to store the ideal interactable once found
        Collider2D matchingInteractable = null;
        float distanceFromMatching = Mathf.Infinity;

        /* Loop through all possible colliders within the range of the player and determine if:
         A) They have the required Tag and B) Which ideal interactable is closest to the player */
        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Interactable"))
            {
                continue;
            }

            float distanceFromPlayer = Vector2.Distance(transform.position, hit.transform.position);
            if (distanceFromPlayer < distanceFromMatching)
            {
                // Store the ideal Collider as the confirmed matching collider and store its distance from the player
                matchingInteractable = hit;
                distanceFromMatching = distanceFromPlayer;
            }
        }

        // Check if the ideal Collider has been assigned a match, if not close the method
        if (matchingInteractable == null)
        {
            interactable = null;
            return false;
        }

        // Store the game object attached to the ideal Collider to the confirmed interactable and return true for this method. 
        interactable = matchingInteractable.gameObject;
        return true;
    }
    
    /// <summary>
    /// Check if the player is Grounded and draw the line so I can see it in the scene
    /// </summary>
    private void CheckIsGrounded()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, rayCastLength, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * rayCastLength, Color.green);
    }

    /// <summary>
    /// Make the player face the direction in which they are recieving input for
    /// </summary>
    /// <param name="moveInput"></param>
    private void FaceInputDirection(float moveInput)
    {
        if (moveInput < 0 && !isFacingLeft)
        {
            transform.Rotate(0.0f, 180f, 0.0f);
            isFacingLeft = true;
        }
        else if (moveInput > 0 && isFacingLeft)
        {
            transform.Rotate(0.0f, 180f, 0.0f);
            isFacingLeft = false;
        }
    }

    /// <summary>
    /// Updates the character's movement state and animation based on input and run action status.
    /// </summary>
    /// <remarks>This method should be called each frame to ensure the character's animation and movement
    /// speed reflect the current input. It sets the appropriate animation states for running and walking, and adjusts
    /// the movement speed accordingly.</remarks>
    private void HorizontalMovementAnimations()
    {
        if(isRunning && moveInputPresent)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
        else if (moveInputPresent && !isRunning)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
        else if (!moveInputPresent)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }

    private void CollectItem(GameObject item)
    {
        inventory.Add(item);
        Debug.Log("Item added to inventory: " + item.name);
        interactable.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, circleCastRadius);
    }
}
