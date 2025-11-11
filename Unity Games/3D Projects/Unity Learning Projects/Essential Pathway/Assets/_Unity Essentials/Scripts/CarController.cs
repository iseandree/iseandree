using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    // Public Variables
    public float speed = 5f;
    public Transform spawnPoint;
    // Private variables 
    private Rigidbody rb; // Reference to the Rigidbody component attached to the player
    private Vector3 movement; // Stores the direction of player movement

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Prevent the car from rotating
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.x > 200f ||  transform.position.x < -55f)
        {
            // If the car ends up out of bounds reset position
            transform.position = spawnPoint.position;

            // Increase the speed of the cars every time they reset
            speed += 2;
        }
    }

    void FixedUpdate()
    {
        movement = transform.forward * speed;
        rb.linearVelocity = movement;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
