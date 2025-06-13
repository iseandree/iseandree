using UnityEngine;

/// <summary>
/// BallThrower is responsible for throwing the basketball based on user input. So far, it handles the swipe gesture from mouse to determine the throw direction and speed.
/// </summary>
public class BallThrower : MonoBehaviour
{
    // Variables
    [SerializeField] private float maxBallSpeed = 40f;
    private Camera mainCamera;
    private Rigidbody basketballRigidbody;
    private InputHandler input;
    private float screenZ;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initialize Variables
        basketballRigidbody = GetComponent<Rigidbody>();
        input = GetComponent<InputHandler>();
        mainCamera = Camera.main;
        screenZ = mainCamera.WorldToScreenPoint(transform.position).z;

        EventManager.OnRelease += HandleRelease; // Subscribe to events for throwing the ball
    }

    // OnDestroy is called when the script instance is being destroyed
    private void OnDestroy()
    {
        EventManager.OnRelease -= HandleRelease; // Unsubscribe from the event to prevent memory leaks
    }

    private void HandleRelease(Vector2 start, Vector2 end, float duration)
    {
        float distance = Vector2.Distance(start, end); // Calculate the distance of the swipe
        if (distance < 0.1f) return; // If the distance is too small, do not throw the ball

        float speed = Mathf.Clamp(distance / duration, 0f, maxBallSpeed); // Calculate the speed based on distance and duration 
        Vector3 startWorld = mainCamera.ScreenToWorldPoint(new Vector3(start.x, start.y, screenZ)); // Convert the start position to world coordinates
        Vector3 endWorld = mainCamera.ScreenToWorldPoint(new Vector3(end.x, end.y, screenZ)); // Convert the end position to world coordinates
        Vector3 throwDirection = (endWorld - startWorld).normalized; // Calculate the direction of the throw using the start and end positions
        basketballRigidbody.isKinematic = false;
        basketballRigidbody.useGravity = true;
        basketballRigidbody.linearVelocity = Vector3.zero;
        basketballRigidbody.AddForce(throwDirection * speed, ForceMode.VelocityChange); // Apply the force to the Rigidbody to throw the ball
        EventManager.OnBallThrown?.Invoke(); // Trigger the ball thrown event
        Debug.Log($"Ball thrown from {startWorld} to {endWorld} with speed {speed} at time {Time.time}");
    }
}