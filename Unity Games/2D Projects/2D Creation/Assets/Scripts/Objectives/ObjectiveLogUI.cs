using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectiveLogUI : MonoBehaviour
{
    [Header("Setup References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Log Data")]
    [SerializeField] private ObjectiveLogSlot[] objectiveLogSlots;
    [SerializeField] private QuestObjectiveSlot[] questObjectiveSlots;
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private TMP_Text objectiveNameText;
    [SerializeField] private TMP_Text objectiveDescriptionText;
    private ObjectiveSO objectiveSO;

    private void Start()
    {
        OpenObjectiveLog();
    }

    public void OpenObjectiveLog()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        SwitchToUIMap();
        StartCoroutine(FocusFirstSlot());
    }

    public void CloseObjectiveLog()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        SwitchToPlayerMap();
    }

    public void HandleQuestSelected(ObjectiveSO objectiveSO)
    {
        this.objectiveSO = objectiveSO;
        Debug.Log($"Selected Objective: {objectiveSO.objectiveName}");
        objectiveNameText.text = objectiveSO.objectiveName;
        objectiveDescriptionText.text = objectiveSO.objectiveDescription;

        DisplayObjective();
        foreach (var objective in objectiveSO.objectives)
        {
            
            Debug.Log($"Objective: {objective.description}");
        }
    }

    private void DisplayObjective()
    {
        for (int i = 0; i < questObjectiveSlots.Length; i++)
        {
            if(i < objectiveSO.objectives.Count)
            {
                var objective = objectiveSO.objectives[i];
                objectiveManager.UpdateObjectiveProgress(objectiveSO, objective);
                int currentAmount = objectiveManager.GetCurrentAmount(objectiveSO, objective);
                string progress = objectiveManager.GetProgressText(objectiveSO, objective);
                bool isComplete = currentAmount >= objective.requiredAmount;

                questObjectiveSlots[i].gameObject.SetActive(true);
                questObjectiveSlots[i].RefreshObjectives(objective.description, progress, isComplete);
            }
            else
            {
                questObjectiveSlots[i].gameObject.SetActive(false);
            }    
        }
    }

    private IEnumerator FocusFirstSlot()
    {
        yield return new WaitForEndOfFrame();

        if(objectiveLogSlots != null && objectiveLogSlots.Length > 0 && objectiveLogSlots[0]!= null)
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(objectiveLogSlots[0].gameObject);
        }
    }

    private void SwitchToUIMap()
    {
        if(playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("UI");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void SwitchToPlayerMap()
    {
        if(playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("Player");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
