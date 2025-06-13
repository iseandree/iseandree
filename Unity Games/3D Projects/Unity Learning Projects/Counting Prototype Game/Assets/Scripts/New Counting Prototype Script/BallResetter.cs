using System.Collections;
using UnityEngine;

/// <summary>
/// BallResetter is responsible for resetting the basketball to its initial position and rotation after a specified delay.
/// </summary>
public class BallResetter : MonoBehaviour
{
    // Variables
    [SerializeField] private float resetDelay = 5f; // Time in seconds before the ball resets
    private Rigidbody basketballRigidbody; 
    private Vector3 initialPosition; 
    private Quaternion initialRotation; 
    private BallThrower ballThrower; 

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initialize Variables
        basketballRigidbody = GetComponent<Rigidbody>();
        ballThrower = GetComponent<BallThrower>();
        initialPosition = transform.position; // Store the initial position of the basketball
        initialRotation = transform.rotation; // Store the initial rotation of the basketball
        EventManager.OnBallThrown += ResetBall; // Subscribe to the ball reset event
    }

    // OnDestroy is called when the script instance is being destroyed
    private void OnDestroy()
    {
        EventManager.OnBallThrown -= ResetBall; // Unsubscribe from the ball reset event to prevent memory leaks
    }

    /// Resets the ball after a delay
    private void ResetBall()
    {
        StartCoroutine(ResetAfterDelay());
    }

    // Resets the ball after a specified delay
    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay); 
        basketballRigidbody.isKinematic = true; 
        basketballRigidbody.useGravity = false; 
        basketballRigidbody.linearVelocity = Vector3.zero; 
        transform.position = initialPosition; // Reset the position of the basketball
        transform.rotation = initialRotation; // Reset the rotation of the basketball
        ballThrower.enabled = true; // Re-enable the BallThrower component to allow throwing again
        Debug.Log("Ball has been reset to its initial position and rotation.");
    }    
}
