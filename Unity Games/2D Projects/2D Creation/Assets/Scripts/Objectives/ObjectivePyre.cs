using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Represents an interactive in-game object "The Pyre" that allows the player to recieve or turn in objectives when in range.
public class ObjectivePyre : MonoBehaviour
{
    // Private Variables
    [SerializeField] private ObjectiveSO objectiveToOffer;
    [SerializeField] private ObjectiveSO objectiveToTurnIn;
    private PlayerInput playerInput;
    private bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = FindFirstObjectByType<PlayerController2D>().GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is in range and activates the "Interact button near the pyre open the objective log
        if (playerInRange && playerInput.actions.FindAction("Interact").WasPressedThisFrame())
        {
            bool canTurnIn = objectiveToTurnIn != null && ObjectiveEvents.IsObjectiveCompleted?.Invoke(objectiveToTurnIn) == true;
            if(canTurnIn)
            {
                ObjectiveEvents.OnObjectiveTurnInRequested?.Invoke(objectiveToTurnIn);
            }
            else
            {
                ObjectiveEvents.OnObjectiveOfferRequested?.Invoke(objectiveToOffer);
            }
        }
    }

    // If the player enters within the pyre's collider they are in range.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // If the player exits from within the pyre's collider they are no longer in range.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
