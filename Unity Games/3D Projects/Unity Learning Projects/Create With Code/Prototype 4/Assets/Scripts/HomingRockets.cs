using UnityEngine;

public class HomingRockets : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float speed = 10.0f;
    private float outOfBounds = 10.0f;
    private Vector3 lookDirection;
    private int enemyCount;
    private GameObject enemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemies = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        //sooooooo the rockets have to track the enemies in the game scene and follow their locations
        // it doesn't matter how many shoot, I guess they will spawn every second while the powerup is activated
        // probably need an array of enemies or use sortmode to find enemies in the scene
        //only shoot instantiate "rockets" when powerup is active 
        if(enemies == null)
        {
            enemies = GameObject.FindGameObjectWithTag("Enemy");
            if(enemies == null)
            {
                Destroy(gameObject);
                return;
            }
        }

        lookDirection = (enemies.transform.position - transform.position).normalized;
        //transform.Rotate()
        transform.forward = lookDirection;
        transform.position += transform.forward * speed * Time.deltaTime;
        // transform.position = enemy position

        if(transform.position.z > outOfBounds || transform.position.z < -outOfBounds)
        {
            Destroy(gameObject);
        }

        if(transform.position.x > outOfBounds || transform.position.x < -outOfBounds)
        {
            Destroy(gameObject);
        }
    }
}
