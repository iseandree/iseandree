using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    // Variables
    private Animator doorAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Animator component assigned to the same object as this script
        doorAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player")) // Make sure the player gameobject has the tag
        {
            if (doorAnimator != null)
            {
                // Trigger the Door Open animation
                doorAnimator.SetTrigger("Door_Open");
            }
        }
    }
}
