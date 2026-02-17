using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float speed = 8.0f;
    [SerializeField] private float jumpForce = 10.0f;
    private float horizontalInput;
    private Rigidbody playerRb;
    private bool isOnGround = false;
    private Animator animator;
    private int foodCollected = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        AllowPlayerJump();
    }

 
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Food"))
        {
            foodCollected++;
            Destroy(other.gameObject);
        }
    }

    // Allow the player to jump using Space and prevent double jumping
    private void AllowPlayerJump()
    {
        if (isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }
    }

    // Move Player horizontally
    private void MovePlayer()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.forward * Time.deltaTime * speed * horizontalInput);
        if (horizontalInput < 0.25f)
        {
            animator.SetFloat("Speed_f", speed);
        }
    }
}
