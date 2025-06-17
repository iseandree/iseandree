using UnityEngine;
using UnityEngine.UI;

public class PerfectReleaseUI : MonoBehaviour
{
    // Variables
    [SerializeField] private Image perfectReleaseMeter; // UI element to indicate perfect release
    [SerializeField] private Image perfectReleaseOverlay;
    [SerializeField] private Color startColor = Color.red; // Color at the start of the hold
    [SerializeField] private Color perfectColor = Color.green; // Color at the end of the hold
    private PerfectReleaseController perfectReleaseController;
    private Vector3 overlayRotation;
    private float perfectReleaseZoneX;
    private float perfectReleaseZoneY;
    private float holdTime;
    private float holdTimePercentage;

    // OnEnable is called when the script is enabled
    private void OnEnable()
    {
        // Subscribe to the event when the script is enabled
        EventManager.OnBallPerfectRelease += HandlePerfectRelease;
    }

    // OnDisable is called when the script is disabled
    private void OnDisable()
    {
        // Unsubscribe from the event when the script is disabled
        EventManager.OnBallPerfectRelease -= HandlePerfectRelease;
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initialize Variables
        perfectReleaseMeter.enabled = true; // Unhide the charge indicator
        perfectReleaseOverlay.enabled = true; // Unhide the perfect release overlay
        perfectReleaseController = GetComponent<PerfectReleaseController>();
        overlayRotation = perfectReleaseOverlay.transform.rotation.eulerAngles; // Store the initial rotation of the overlay
        perfectReleaseZoneX = perfectReleaseController.GetPerfectReleaseZone().x;
        perfectReleaseZoneY = perfectReleaseController.GetPerfectReleaseZone().y;
        perfectReleaseOverlay.fillAmount = perfectReleaseZoneY - perfectReleaseZoneX;
        overlayRotation.z = perfectReleaseZoneX * 360f; // Set the z rotation based on the perfect release zone x value     
    }

    // Update is called once per frame
    void Update()
    {
        perfectReleaseMeter.fillAmount = holdTime; // Update the charge indicator fill amount based on hold time
        holdTime = perfectReleaseController.GetNormalizedHoldTime();
        
        // Color Lerp
        if (holdTime < perfectReleaseZoneX)
        {
            // Starting toward perfect zone
            holdTimePercentage = holdTime / perfectReleaseZoneX; // Calculate the percentage of the hold time within the perfect release zone
            perfectReleaseMeter.color = Color.Lerp(startColor, Color.yellow, holdTimePercentage); // Interpolate color from startColor to perfectColor
        }
        else if (holdTime <= perfectReleaseZoneY)
        {
            // Going through perfect zone
            holdTimePercentage = (holdTime - perfectReleaseZoneX) / (perfectReleaseZoneY - perfectReleaseZoneX); // Calculate the percentage of the hold time within the perfect release zone
            perfectReleaseMeter.color = Color.Lerp(Color.yellow, perfectColor, holdTimePercentage);
        }
        else
        {
            // Past the zone
            perfectReleaseMeter.color = startColor;
        }
    }

    // HandlePerfectRelease is called when the perfect release event is triggered
    private void HandlePerfectRelease(float holdTime, bool isPerfect)
    {
        perfectReleaseMeter.fillAmount = 0;
    }
}
