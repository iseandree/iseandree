using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private GameObject projectilePrefab;
    private Vector3 projectilePos;
    private float xRange = 15.0f;
    private float zRangeMin = 0.0f;
    private float zRangeMax = 7.0f;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        projectilePos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Launch the projectile from the player
            Instantiate(projectilePrefab, projectilePos, projectilePrefab.transform.rotation);
        }
    }

    private void MovePlayer()
    {
        // Player Input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);

        // Keep the player within the bounds
        // X Bounds
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        // Z Bounds
        if (transform.position.z < zRangeMin)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRangeMin);
        }

        if (transform.position.z > zRangeMax)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRangeMax);
        }
    }
}
