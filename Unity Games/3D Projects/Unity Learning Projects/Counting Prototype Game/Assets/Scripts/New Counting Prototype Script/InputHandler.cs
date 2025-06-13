using UnityEngine;

/// <summary>
/// InputHandler is responsible for handling user input related to the basketball game. It allows the player to pick up and drop the ball using mouse input.
/// </summary>
public class InputHandler : MonoBehaviour
{
    // Variables
    [SerializeField] private float rayDistance = 100f;
    private Camera mainCamera;
    private Vector2 pickUpPosition;
    private float pickUpTime;    
    private float dropTime;
    private bool isHolding;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle Input for Picking Up and Dropping the Ball - If left mouse button is pressed, cast a ray from the camera to the mouse position
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, rayDistance) && hit.transform == transform) // Check if the ray hits the object this script is attached to
            {
                pickUpPosition = Input.mousePosition;
                pickUpTime = Time.time;
                isHolding = true;
                EventManager.PickUpBall(pickUpPosition, pickUpTime);
                Debug.Log($"Picked up at {pickUpPosition} @ {pickUpTime}");
            }
        }
        else if(Input.GetMouseButtonUp(0) && isHolding) // If left mouse button is released and we are holding the ball release it
        {
            dropTime = Time.time;
            Vector2 dropPosition = Input.mousePosition;
            EventManager.DropBall(pickUpPosition, dropPosition, dropTime - pickUpTime);
            isHolding = false;
            Debug.Log($"Dropped at {dropPosition} @ {dropTime}, Duration: {dropTime - pickUpTime}");
        }
    }
}
