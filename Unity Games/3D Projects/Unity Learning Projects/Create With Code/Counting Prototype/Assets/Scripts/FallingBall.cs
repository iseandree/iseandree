using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FallingBall : MonoBehaviour
{
    // Serialize Field Variables
    [SerializeField] private float horizontalMovementSpeed;
    [SerializeField] private ParticleSystem bounceParticles;
    [SerializeField] private float zBound = 6.50f;

    // Private Variables
    private ConstantForce ballGravity;
    private bool canFlipDirection = false;
    private GameBallSpawner gameBallSpawner;

    // Public Variables
    public bool isDropped = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballGravity = GetComponent<ConstantForce>();
        gameBallSpawner = GameObject.Find("Game Ball Spawner").GetComponent<GameBallSpawner>();
        ballGravity.enabled = false;
        horizontalMovementSpeed = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMovement();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ballGravity.enabled = true;
            isDropped = true;
            horizontalMovementSpeed = 0;
        }
    }

    // Pre ball drop the ball should move horizontally 
    private void HorizontalMovement()
    {
        if (transform.position.z > zBound)
        {
            canFlipDirection = true;
        }

        if (transform.position.z < -zBound)
        {
            canFlipDirection = false;
        }

        if (canFlipDirection)
        {
            transform.Translate(0, 0, -horizontalMovementSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(0, 0, horizontalMovementSpeed * Time.deltaTime);
        }
    }

    public void ResetBall()
    {
        gameBallSpawner.SpawnGameBall();
        isDropped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Out Of Bounds"))
        {
            Destroy(gameObject);
            ResetBall();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall Node"))
        {
            bounceParticles.Play();
        }       
    }
}
