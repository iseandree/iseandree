using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ThrowBall : MonoBehaviour
{
    // Variables
    private GameObject basketball;
    private Camera mainCamera;
    private Rigidbody basketballRigidbody;
    private float startTime;
    private float endTime;
    private float ballScreenPositionZ; // Z position of the ball in screen space
    private float ballVelocity = 0.0f;
    private float ballSpeed = 100.0f;
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

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupBall();
        ballScreenPositionZ = mainCamera.WorldToScreenPoint(basketball.transform.position).z; // Get the Z position of the ball in screen space
    }

    // Update is called once per frame
    void Update()
    {
           

        if (isHoldingBall)
        {
            PickupBall();
        }

        if(isBallThrown)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                if (hit.transform == basketball.transform)
                {
                    startTime = Time.time;
                    startPosition = Input.mousePosition;
                    isHoldingBall = true;
                    Debug.Log("Ball picked up at: " + startPosition);
                    Debug.Log("Start Time: " + startTime);
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
                Vector3 startWorld = mainCamera.ScreenToWorldPoint(new Vector3(startPosition.x, startPosition.y, ballScreenPositionZ));
                Vector3 endWorld = mainCamera.ScreenToWorldPoint(new Vector3(endPosition.x, endPosition.y, ballScreenPositionZ));
                Vector3 throwDirection = (endWorld - startWorld).normalized;
                swipeAngle = throwDirection;
                basketballRigidbody.AddForce(throwDirection * ballSpeed, ForceMode.VelocityChange);
                basketballRigidbody.isKinematic = false; // Set the Rigidbody to non-kinematic to allow physics interactions
                basketballRigidbody.useGravity = true;
                isBallThrown = true;
                isHoldingBall = false;
                Invoke("ResetBall", resetTimer); // Reset the ball after 5 seconds
            }
            else
            {
                ResetBall();
            }
            Debug.Log("Ball released at: " + endPosition);
            Debug.Log("End Time: " + endTime);
            Debug.Log("Swipe Distance: " + swipeDistance);
            Debug.Log("Swipe Time: " + swipeTime);
            Debug.Log("Ball Speed: " + ballSpeed);
            Debug.Log("Swipe Angle: " + swipeAngle);
        }
    }

    // This method sets up the basketball and its Rigidbody component
    private void SetupBall()
    {
        basketball = GameObject.FindGameObjectWithTag("Basketball");
        basketballRigidbody = basketball.GetComponent<Rigidbody>();
        ResetBall();
    }

    // This method is called to reset the ball's position and state
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
        basketballRigidbody.isKinematic = true; // Set the Rigidbody to kinematic to prevent physics interactions
        basketballRigidbody.useGravity = false;
        basketball.transform.position = transform.position; // Reset position of the ball
    }

    // This method is called to pick up the ball and move it with the mouse
    private void PickupBall()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane * ballDistanceFromCamera;
        newPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Move the ball
        basketball.transform.position = Vector3.Lerp(basketball.transform.position, newPosition, mouseCameraSpeedThreshold * Time.deltaTime);
    }
}
