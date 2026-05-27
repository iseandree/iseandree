using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectivePyre : MonoBehaviour
{
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
        if(playerInRange && playerInput.actions.FindAction("Open Objective Log").WasPressedThisFrame())
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
