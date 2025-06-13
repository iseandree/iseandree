using UnityEngine;

/// <summary>
/// BallDragger is responsible for dragging the basketball around the scene based on user input. So far, it handles the mouse input to allow the player to drag the ball smoothly.
/// </summary>
public class BallDragger : MonoBehaviour
{
    // Variables 
    [SerializeField] private float smoothing = 10f;
    private Camera mainCamera;
    private Rigidbody basketballRigidbody;
    private InputHandler input;
    private float screenZ;
    private bool isDragging = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initialize Variables
        basketballRigidbody = GetComponent<Rigidbody>();
        input = GetComponent<InputHandler>();
        mainCamera = Camera.main;
        screenZ = mainCamera.WorldToScreenPoint(transform.position).z;

        // Subscribe to events for picking up and dropping the ball
        EventManager.OnPickUp += StartDrag;
        EventManager.OnRelease += StopDrag;
    }

    // OnDestroy is called when the script instance is being destroyed
    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        EventManager.OnPickUp -= StartDrag;
        EventManager.OnRelease -= StopDrag;
    }

    // StartDrag is called when the ball is picked up
    private void StartDrag(Vector2 position, float time)
    {
        basketballRigidbody.isKinematic = true; // Make the Rigidbody kinematic to prevent physics interactions while dragging
        basketballRigidbody.useGravity = false; // Disable gravity while dragging
        isDragging = true; // Set dragging state to true and allow ball to be dragged
    }

    // StopDrag is called when the ball is dropped
    private void StopDrag(Vector2 pickUpPosition, Vector2 dropPosition, float duration)
    {
        isDragging = false; // Set dragging state to false and prevent ball from being dragged
    }

    // Update is called once per frame
    private void Update()
    {
        if(!isDragging) return; // If not dragging, exit the method

        Vector3 mousePosition = Input.mousePosition; // Get the current mouse position
        mousePosition.z = screenZ; // Set the z position to the distance from the camera to the ball
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition); // Convert the mouse position to world coordinates
        transform.position = Vector3.Lerp(transform.position, worldPosition, Time.deltaTime * smoothing); // Smoothly move the ball to the mouse position
    }
}
