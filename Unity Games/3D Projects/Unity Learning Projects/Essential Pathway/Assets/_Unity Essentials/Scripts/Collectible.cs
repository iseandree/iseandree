using UnityEngine;

public class Collectible : MonoBehaviour
{
    //Variables
    public float rotationSpeed;
    public GameObject onCollectEffect;
    public AudioSource audioSource;
    public AudioClip collectedSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    // OnTriggerEnter is called when one object enters a trigger collider zone of another object
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // Play the sound when collected
            audioSource.PlayOneShot(collectedSound);

            // Destroy the collectible
            Destroy(gameObject);

            // Instantiate the particle effect
            Instantiate(onCollectEffect, transform.position, transform.rotation);
        }
    }
}
