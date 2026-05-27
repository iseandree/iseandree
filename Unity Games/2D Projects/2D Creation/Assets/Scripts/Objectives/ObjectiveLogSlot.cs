using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectiveLogSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveNameText;
    [SerializeField] private TMP_Text objectivePriorityText;

    public ObjectiveSO currentObjective;
    public ObjectiveLogUI objectiveLogUI;

    public void SetObjective(ObjectiveSO objectiveSO)
    {
        currentObjective = objectiveSO;

        objectiveNameText.text = objectiveSO.objectiveName;
        objectivePriorityText.text = objectiveSO.objectivePriority;
        gameObject.SetActive(true);
    }
    
    public void ClearSlot()
    {
        currentObjective = null;
        gameObject.SetActive(false);
    }

    public void OnSlotSelected()
    {
        UpdateLogDetails();
    }

    public void OnSelect(BaseEventData eventData)
    {
        UpdateLogDetails();
    }

    private void UpdateLogDetails()
    {
        if(objectiveLogUI != null && currentObjective != null)
        {
            objectiveLogUI.HandleQuestSelected(currentObjective);
        }
    }
}
