using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isNear;
    [SerializeField] private float jumpCooldown = 0.50f;
    [SerializeField] private float circleCastRadius = 0.75f;
    [SerializeField] private float rayCastLength = 2f;

    // Private Variables
    private float lastJumpTime;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private Rigidbody2D rb;
    private LayerMask groundLayer;
    private LayerMask borderLayer;
    private GameObject interactable;
    private bool isFacingLeft = false;
    private bool isMoving = false;
    private bool isMovingLeft;
    private bool isMovingRight;
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
        interactAction = playerInput.actions.FindAction("Interact");
        lastJumpTime = -jumpCooldown;

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is near a collectible object
        if (DetectInteractable() && interactAction.WasPressedThisFrame())
        {
            CollectItem(interactable);
        }

    }

    private void FixedUpdate()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, rayCastLength, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * rayCastLength, Color.green);
        Debug.Log(isGrounded);

        // Get horizontal input
        float moveInput = moveAction.ReadValue<Vector2>().x;
        
        // Apply Movement Force
        rb.AddForce(Vector2.right * moveInput * moveSpeed, ForceMode2D.Force);

        // Update 'isWalking' based on whether there is any input (regardless of direction)
        bool inputPresent = Mathf.Abs(moveInput) > 0.1f;
        animator.SetBool("isWalking", inputPresent);

        // Handle flipping separately
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

        // Prevent the player from spamming jump
        if (jumpAction.IsPressed() && isGrounded && Time.time >= lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
            animator.SetBool("isJumping", true);
        }
        else if(isGrounded)
        {
            animator.SetBool("isJumping", false);
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
