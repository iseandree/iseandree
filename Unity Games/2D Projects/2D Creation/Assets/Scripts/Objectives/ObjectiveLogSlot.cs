using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Represents a UI slot for displaying and managing an objective entry within an objective log interface.
public class ObjectiveLogSlot : MonoBehaviour
{
    // UI Variables
    [SerializeField] private TMP_Text objectiveNameText;
    [SerializeField] private TMP_Text objectivePriorityText;
    private ObjectiveSO currentObjective;
    private ObjectiveLogUI objectiveLogUI;

    // Set the objective related text of this slot from the passed objective
    public void SetObjective(ObjectiveSO objectiveSO)
    {
        currentObjective = objectiveSO;

        objectiveNameText.text = objectiveSO.objectiveName;
        objectivePriorityText.text = objectiveSO.objectivePriority;
        gameObject.SetActive(true);
    }
    
    // Clear the slot as it and make it empty when need be
    public void ClearSlot()
    {
        currentObjective = null;
        gameObject.SetActive(false);
    }

    // Handle the selection of this slot and update the details related to it
    public void OnSlotSelected()
    {
        UpdateLogDetails();
    }

    // Update the log UI details with the information of the objective that resides in this slot
    private void UpdateLogDetails()
    {
        if(objectiveLogUI != null && currentObjective != null)
        {
            objectiveLogUI.HandleObjectiveSelected(currentObjective);
        }
    }
}
