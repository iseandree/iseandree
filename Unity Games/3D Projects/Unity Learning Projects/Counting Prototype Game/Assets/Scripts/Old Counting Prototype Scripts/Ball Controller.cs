using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class BallController : MonoBehaviour
{
    // Variables
    private bool isFrozen = false;
    public float angleOffset = 50f;
    private float ballSpeed = 0f;
    public float minSwipeDistance = 30f;
    public float maxSwipeTime = 0.5f;
    public float maxThrowForce = 20f;
    public float throwForceMultiplier = 10f;
    public InputAction shoot;
    private Rigidbody ballRigidbody;
    private Vector2 startSwipe;
    private Vector2 endSwipe;
    private Vector3 angle;
    private Vector3 resetPosition;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        resetPosition = transform.position;
        FreezeBall();
    }

    // Update is called once per frame
    void Update()
    {
       DetectSwipe();
    }

    // Freeze the ball
    void FreezeBall()
    {
        isFrozen = true;
        ballRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Unfreeze the ball
    void UnfreezeBall()
    {
        isFrozen = false;
        ballRigidbody.constraints = RigidbodyConstraints.None;
    }

    // Detect swipe input
    void DetectSwipe()
    {
        // Detect touch or mouse input
        if (Input.GetMouseButtonDown(0))
        {
            startSwipe = Input.mousePosition;
            Time.timeScale = Time.realtimeSinceStartup;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endSwipe = Input.mousePosition;
            float swipeDistance = (endSwipe - startSwipe).magnitude;
            float swipeTime = Time.realtimeSinceStartup - Time.timeScale;

            if (swipeDistance >= minSwipeDistance && swipeTime <= maxSwipeTime)
            {
                UnfreezeBall();
                CalculateSpeed(swipeDistance, swipeTime);
                CalculateAngle();
                ShootBall();
            }
        }
    }

    // Calculate the ball's throw angle
    void CalculateAngle()
    {
        angle = Camera.main.ScreenToWorldPoint(new Vector3(endSwipe.x, endSwipe.y + angleOffset, Camera.main.nearClipPlane + 5f));
    }

    // Calculate the ball's speed based on swipe distance and time
    void CalculateSpeed(float swipeDistance, float swipeTime)
    {
        float ballVelocity = swipeDistance / swipeTime;
        ballSpeed = Mathf.Min(ballVelocity * throwForceMultiplier, maxThrowForce);
    }

    // Apply force to the ball in the calculated direction and speed
    void ShootBall()
    {
        Vector3 direction = new Vector3(angle.x, angle.y / 3f, angle.z * 2f).normalized;
        ballRigidbody.AddForce(direction * ballSpeed, ForceMode.Impulse);
        ballRigidbody.useGravity = true;
    }
}
