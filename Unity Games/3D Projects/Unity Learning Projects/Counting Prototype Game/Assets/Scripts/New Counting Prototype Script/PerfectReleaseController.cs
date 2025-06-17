using UnityEngine;

public class PerfectReleaseController : MonoBehaviour
{
    // Variables
    [SerializeField] private float maxHoldTime;
    [SerializeField] private Vector2 perfectReleaseZone = new Vector2(0.8f, 0.9f);
    private float holdTime;
    private float holdStartTime;
    private bool isHolding = false;
    private float normalized;
    private bool isPerfect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Handle Input for Perfect Release - If the player holds down the fire button, start timing
        if (Input.GetButtonDown("Fire1"))
        {
            // Record the start time of the hold and set the holding state to true
            holdStartTime = Time.time;
            isHolding = true;
            Debug.Log("Holding the ball..." + holdStartTime);
        }
        else if (Input.GetButtonUp("Fire1") && isHolding)   // If the player releases the fire button and is currently holding the ball, calculate the hold time
        {
            // Calculate the hold time and determine if it falls within the perfect release zone
            holdTime = Time.time - holdStartTime;
            holdTime = Mathf.Clamp(holdTime, 0f, maxHoldTime);
            normalized = holdTime / maxHoldTime;  // Normalize the hold time to a value between 0 and 1
            isPerfect = (normalized >= perfectReleaseZone.x && normalized <= perfectReleaseZone.y);
            EventManager.OnBallPerfectRelease?.Invoke(holdTime, isPerfect);
            isHolding = false;
            Debug.Log($"Released the ball after {holdTime} seconds. Perfect Release: {isPerfect}");
        }
    }

    public float GetNormalizedHoldTime()
    {
        if (!isHolding)
        {
            return 0f; // Return 0 if not holding the ball
        }
        return Mathf.Clamp01((Time.time - holdStartTime) / maxHoldTime); ; // Return the normalized hold time
    }

    public Vector2 GetPerfectReleaseZone()
    {
        return perfectReleaseZone; // Return the perfect release zone
    }
}
