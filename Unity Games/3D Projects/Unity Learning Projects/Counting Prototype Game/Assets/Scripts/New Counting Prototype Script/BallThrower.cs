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

    private void HandleRelease(Vector2 startPos, Vector2 endPos, float duration)
    {
        float distance = Vector2.Distance(startPos, endPos);
        if (distance < 0.1f) return;

        float speed = Mathf.Clamp(distance / duration, 0f, maxBallSpeed);
        Ray ray = mainCamera.ScreenPointToRay(endPos);
        Vector3 baseDir = ray.direction.normalized;

        // add an upward lift factor
        float arcFactor = 0.5f;
        Vector3 throwDir = (baseDir + Vector3.up * arcFactor).normalized;

        basketballRigidbody.isKinematic = false;
        basketballRigidbody.useGravity = true;
        basketballRigidbody.linearVelocity = Vector3.zero;
        basketballRigidbody.AddForce(
            throwDir * speed,
            ForceMode.VelocityChange
        );

        EventManager.OnBallThrown?.Invoke();
    }
}