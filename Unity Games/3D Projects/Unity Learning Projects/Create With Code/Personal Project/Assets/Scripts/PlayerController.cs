using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float speed = 8.0f;
    [SerializeField] private float jumpForce = 10.0f;
    private float horizontalInput;
    private Rigidbody playerRb;
    private bool isOnGround = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        playerRb.AddForce(Vector3.forward * speed * horizontalInput);
        
        if(isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}
