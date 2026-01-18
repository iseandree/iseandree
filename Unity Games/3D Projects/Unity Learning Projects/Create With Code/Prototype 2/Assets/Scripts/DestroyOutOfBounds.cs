using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float topBound = 30.0f;
    [SerializeField] private float lowerBound = -10.0f;
    [SerializeField] private float sideBounds = 25.0f;
    private int lives = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If an object goes past the player's view, remove that object
        // Vertically
        if (transform.position.z > topBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < lowerBound)
        {
            Debug.Log("Life Lost! -1");
            GameManager.LoseLife(lives);
            Destroy(gameObject);
        }
        
        // Horizontally
        if(transform.position.x < -sideBounds)
        {
            Debug.Log("Life Lost! -1");
            GameManager.LoseLife(lives);
            Destroy(gameObject);
        }
        else if (transform.position.x > sideBounds)
        {
            Debug.Log("Life Lost! -1");
            GameManager.LoseLife(lives);
            Destroy(gameObject);
        }
    }
}
