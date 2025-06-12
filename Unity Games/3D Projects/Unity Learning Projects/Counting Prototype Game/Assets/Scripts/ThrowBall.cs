using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ThrowBall : MonoBehaviour
{
    // Variables
    private GameObject basketball;
    private Rigidbody basketballRigidbody;
    private float startTime;
    private float endTime;
    private float ballVelocity = 0.0f;
    private float ballSpeed = 0.0f;
    [SerializeField] private float maxBallSpeed = 40.0f;
    private float swipeDistance;
    [SerializeField] private float minSwipeDistance = 50.0f; // Minimum swipe distance to throw the ball
    private float swipeTime;
    [SerializeField] private float maxSwipeTime = 0.5f; // Maximum time allowed for a swipe to be considered valid
    private float minSwipeTime = 0.0f;
    [SerializeField] private float smoothing = 0.7f;
    [SerializeField] private float resetTimer = 5.0f; // Time after which the ball resets if not thrown
    [SerializeField] private float ballDistanceFromCamera = 5.0f; // Distance from the camera to the ball when held
    [SerializeField] private float mouseCameraSpeedThreshold = 50f; // Speed threshold for mouse movement keeping the ball following the mouse
    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector3 swipeAngle;
    private Vector3 newPosition;
    private bool isBallThrown;
    private bool isHoldingBall;
    [SerializeField] private float raycastDistance = 100f; // Distance for raycasting


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupBall();
    }

    // Update is called once per frame
    void Update()
    {
        if(isHoldingBall)
        {
            PickupBall();
        }

        if(isBallThrown)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                if (hit.transform == basketball.transform)
                {
                    startTime = Time.time;
                    startPosition = Input.mousePosition;
                    isHoldingBall = true;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTime = Time.time;
            endPosition = Input.mousePosition;
            swipeDistance = (endPosition - startPosition).magnitude;
            swipeTime = endTime - startTime;
            if (swipeDistance > minSwipeDistance && swipeTime < maxSwipeTime)
            {
                // Calculate the swipe angle
                //swipeAngle = (endPosition - startPosition).normalized;
                //ballSpeed = Mathf.Clamp(swipeDistance / swipeTime, 0.0f, maxBallSpeed);
                //ThrowBasketball();
                //isBallThrown = true;
                CalculateBallSpeed();
                CalculateAngle();
                basketballRigidbody.AddForce(new Vector3((swipeAngle.x * ballSpeed), (swipeAngle.y * ballSpeed), (swipeAngle.z * ballSpeed)));
                basketballRigidbody.useGravity = true;
                isBallThrown = true;
                isHoldingBall = false;
                Invoke("ResetBall", resetTimer); // Reset the ball after 5 seconds
            }
            else
            {
                ResetBall();
            }
        }
    }

    private void ThrowBasketball()
    {

    }

    private void SetupBall()
    {
        basketball = GameObject.FindGameObjectWithTag("Basketball");
        basketballRigidbody = basketball.GetComponent<Rigidbody>();
        ResetBall();
    }

    private void ResetBall()
    {
        // Reset all variables to their initial state
        swipeAngle = Vector3.zero;
        endPosition = Vector2.zero;
        startPosition = Vector2.zero;
        ballSpeed = 0.0f;
        startTime = 0.0f;
        endTime = 0.0f;
        swipeDistance = 0.0f;
        swipeTime = 0.0f;
        isBallThrown = false;
        isHoldingBall = false;
        basketballRigidbody.linearVelocity = Vector3.zero;
        basketballRigidbody.useGravity = false;
        basketball.transform.position = transform.position; // Reset position of the ball
    }

    private void PickupBall()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane * ballDistanceFromCamera;
        newPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Move the ball
        basketball.transform.position = Vector3.Lerp(basketball.transform.position, newPosition, mouseCameraSpeedThreshold * Time.deltaTime);
    }

    private void CalculateAngle()
    {
        swipeAngle = Camera.main.ScreenToWorldPoint(new Vector3(endPosition.x, endPosition.y, (Camera.main.nearClipPlane + ballDistanceFromCamera)));
    }

    private void CalculateBallSpeed()
    {
        if(swipeTime > minSwipeTime)
        {
            ballVelocity = swipeDistance / (swipeDistance - swipeTime);
        }

        ballSpeed = ballVelocity * maxBallSpeed;
        
        if(ballSpeed >= maxBallSpeed)
        {
            ballSpeed = maxBallSpeed;
        }
        else if (ballSpeed <= maxBallSpeed)
        {
            ballSpeed += maxBallSpeed;
        }
        swipeTime = minSwipeTime;
    }

}
