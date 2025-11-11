using UnityEngine;

[ExecuteAlways] // Optional: allows it to run in Editor mode for preview
public class DayNightCycle : MonoBehaviour
{
    [Tooltip("How many real-time seconds one in-game day lasts.")]
    public float dayLengthInSeconds = 120f; // Default: 2 minutes per day

    [Tooltip("The axis around which the light rotates (default is Vector3.right).")]
    public Vector3 rotationAxis = Vector3.right;

    [Tooltip("Offset the start rotation (0 = sunrise at horizon).")]
    public float startRotation = 0f;

    private void Update()
    {
        if (dayLengthInSeconds <= 0f) return;

        // How many degrees to rotate per second
        float degreesPerSecond = 360f / dayLengthInSeconds;

        // Apply rotation
        transform.Rotate(rotationAxis, degreesPerSecond * Time.deltaTime, Space.Self);

        // Optional: reset rotation if needed
        // transform.localRotation = Quaternion.Euler((Time.time / dayLengthInSeconds) * 360f + startRotation, 0f, 0f);
    }

    private void OnValidate()
    {
        if (rotationAxis == Vector3.zero)
            rotationAxis = Vector3.right; // fallback to default axis
    }
}
