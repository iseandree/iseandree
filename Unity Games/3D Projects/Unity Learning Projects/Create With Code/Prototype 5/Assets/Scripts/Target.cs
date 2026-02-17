using UnityEngine;

public class Target : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float minSpeed = 12.0f;
    [SerializeField] private float maxSpeed = 16.0f;
    [SerializeField] private float maxTorque = 10.0f;
    [SerializeField] private float xRange = 4.0f;
    [SerializeField] private float ySpawnPos = -6.0f;
    [SerializeField] private int pointValue;
    [SerializeField] public ParticleSystem explosionParticle;
    private Rigidbody targetRb;
    private GameManager gameManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque());
        transform.position = RandomSpawnPos();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Randomly Generate the Spawn Position of the Targets
    private Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), -ySpawnPos);
    }

    // Randomly Generate the Torque applied of the Targets
    private float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    // Randomly Generate the Force applied of the Targets
    private Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    public void DestroyObject()
    {
        if (gameManager.isGameActive)
        {
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            Destroy(gameObject);
            gameManager.UpdateScore(pointValue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (!other.gameObject.CompareTag("Bad"))
        {
            gameManager.LoseLife();
        }
    }
}
