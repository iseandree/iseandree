using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    // Public variables
    public bool gameOver = false;
    public bool isLowEnough = false;
    public float floatForce;
    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;

    // Private variables
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;
    private AudioSource playerAudio;
    private float upperBound = 15.0f;
    private float lowerBound = -2.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && isLowEnough)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

        // Bound the player within the game view
        if (transform.position.y > upperBound)
        {
            isLowEnough = false;
        }

        if (transform.position.y < lowerBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
            
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

        else if(other.gameObject.CompareTag("Ground"))
        {
            playerRb.AddForce(Vector3.up * floatForce * 8);
            playerAudio.PlayOneShot(bounceSound, 0.5f);
        }

    }

}
