using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float powerupStrength = 15.0f;
    [SerializeField] public bool hasRegularPowerup;
    [SerializeField] public bool hasRocketPowerup;
    [SerializeField] public bool hasSmashPowerup;
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private GameObject homingRocket;
    [SerializeField] private GameObject rocketSpawnPoint;
    [SerializeField] private float dropForce = 5.0f;
    [SerializeField] private float stopTime = 0.5f;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private bool isGrounded;
    private EnemyController[] enemies;
    private bool canGroundPound = false;
    private float gravityScale = 1.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private Vector3 powerIndicatorOffset = new Vector3(0.0f, -0.5f, 0);
    private float shootTimer;
    private bool canShoot = false;
    private Vector3 rocketOffest = new Vector3(0.0f, 0.0f, 0.75f);
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(forwardInput * speed * focalPoint.transform.forward);
        powerupIndicator.transform.position = transform.position + powerIndicatorOffset;
        enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        if (canShoot && shootTimer < Time.time)
        {
            ShootRockets();
            shootTimer = Time.time + 1;
        }

        if(hasSmashPowerup && Input.GetKeyDown(KeyCode.Space))
        {
            canGroundPound = true;
        }
    }

    private void FixedUpdate()
    {
        if(canGroundPound == true)
        {
            GroundPound();
            CompleteGroundAndPound();
        }
    }

    private void ShootRockets()
    {
        if (hasRocketPowerup)
        {
            Instantiate(homingRocket, rocketSpawnPoint.transform.position + rocketOffest, homingRocket.transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Regular Powerup"))
        {
            hasRegularPowerup = true;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }

        if (other.CompareTag("Rocket Powerup"))
        {
            hasRocketPowerup = true;
            canShoot = true;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }

        if (other.CompareTag("Smash Powerup"))
        {
            hasSmashPowerup = true;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasRegularPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name + "with the powerup set to " + hasRegularPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }

        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasRegularPowerup = false;
        hasRocketPowerup = false;
        canShoot = false;
        powerupIndicator.SetActive(false);
    }

    private void GroundPound()
    {
        playerRb.AddForce(Vector3.up * speed * 6, ForceMode.Impulse);
        StopAndSpin();
        StartCoroutine(DropAndSmash());
        if(isGrounded == true)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
                }
            }
            CompleteGroundAndPound();
        }
    }

    private void StopAndSpin()
    {
        ClearForces();
        playerRb.useGravity = false;
        isGrounded = false;
    }

    private IEnumerator DropAndSmash()
    {
        yield return new WaitForSeconds(stopTime);
        playerRb.AddForce(Vector3.down * dropForce, ForceMode.Impulse);   
    }

    private void CompleteGroundAndPound()
    {
        playerRb.useGravity = true;
        canGroundPound = false;
        hasSmashPowerup = false;
        powerupIndicator.SetActive(false);
    }

    private void ClearForces()
    {
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }
}
