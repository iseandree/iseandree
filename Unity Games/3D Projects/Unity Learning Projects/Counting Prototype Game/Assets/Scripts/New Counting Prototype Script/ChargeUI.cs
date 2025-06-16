using UnityEngine;
using UnityEngine.UI;

public class ChargeUI : MonoBehaviour
{
    // Variables
    private PerfectReleaseController perfectReleaseController;
    [SerializeField] private Image chargeIndicator; // UI element to indicate charge level


    private void Awake()
    {
        // Initialize Variables
        chargeIndicator.enabled = true; // Unhide the charge indicator
        perfectReleaseController = GetComponent<PerfectReleaseController>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        chargeIndicator.fillAmount += perfectReleaseController.GetNormalizedHoldTime(); // Update the charge indicator fill amount based on hold time
    }
}
