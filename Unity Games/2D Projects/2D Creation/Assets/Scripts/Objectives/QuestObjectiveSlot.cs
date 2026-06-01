using TMPro;
using UnityEngine;

// Represents a UI slot that displays a quest objective and its progress within the game interface.
public class QuestObjectiveSlot : MonoBehaviour
{
    // Private UI Variables
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private TMP_Text trackingText;

    // Update the UI of the objective slot based on whether or not the objective is completed or not
    public void RefreshObjectives(string description, string progressText, bool isComplete)
    {
        objectiveText.text = description;
        trackingText.text = progressText;
        Color color = isComplete ? Color.gray : Color.white;
        objectiveText.color = color;
        trackingText.color = color;
    }
}
