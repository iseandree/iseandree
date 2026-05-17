using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;



public class NPCAI : MonoBehaviour
{
    // Serialize Field
    [SerializeField] private float leftPatrolX;
    [SerializeField] private float rightPatrolX;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float minPauseTime;
    [SerializeField] private float maxPauseTime;
    [SerializeField] private float minWalkTime;
    [SerializeField] private float maxWalkTime;

    // Private Variables
    private Rigidbody2D rb;
    private int facingDirection = -1;
    private float randomTime;
    private float timer;
    private bool isWalking = true;
    private bool isFlipping = false;
    private Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        randomTime = Random.Range(minWalkTime, maxWalkTime);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= randomTime)
        {
            StateChange();
        }
        if (!isFlipping && (transform.position.x < leftPatrolX || transform.position.x > rightPatrolX))
        {
            StartCoroutine(Flip());
        }

        if (isWalking)
        {
            rb.linearVelocity = Vector2.right * facingDirection * moveSpeed;
            
        }
        
        animator.SetBool("isWalking", isWalking);
    }

    private void StateChange()
    {
        isWalking = !isWalking;
        if (isWalking)
        {
            randomTime = Random.Range(minWalkTime, maxWalkTime);
        }
        else
        {
            randomTime = Random.Range(minPauseTime, maxPauseTime);
        }
        timer = 0;
    }

    IEnumerator Flip()
    {
        isFlipping = true;
        transform.Rotate(0, 180, 0);
        facingDirection *= -1;
        yield return new WaitForSeconds(0.5f);
        isFlipping = false;
    }
}
