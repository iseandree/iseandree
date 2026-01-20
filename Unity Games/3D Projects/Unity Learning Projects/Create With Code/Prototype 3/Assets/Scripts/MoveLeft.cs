using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Private variables
    [SerializeField] private float speed = 30.0f;
    [SerializeField] private float leftBound = -10.0f;
    private PlayerController playerControllerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();    
    }

    // Update is called once per frame
    void Update()
    {
        // If game is not over move the object left-ward
        if (playerControllerScript.gameOver == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        // If the game object is an Obstacle and is positioned less than the left bound, destroy it
        if(transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
