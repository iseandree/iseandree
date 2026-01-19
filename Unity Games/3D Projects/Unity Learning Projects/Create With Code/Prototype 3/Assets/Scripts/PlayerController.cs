using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Private variables
    private Rigidbody playerRb;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float gravityModifier;
    [SerializeField] private bool isGrounded = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
}
