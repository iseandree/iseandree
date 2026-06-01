using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Manages the user interface for displaying, accepting and completing objectives within the game. Handles
// the presentation and interaction logic for the objective log, including updating UI elements and responding to
// objective-related events.
public class ObjectiveLogUI : MonoBehaviour
{
    // SerializeField Variables that help control UI
    [Header("Setup References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CanvasGroup objectiveCanvasGroup;
    [SerializeField] private CanvasGroup acceptCanvasGroup;
    [SerializeField] private CanvasGroup completeCanvasGroup;
    [SerializeField] private TMP_Text objectiveNameText;
    [SerializeField] private TMP_Text objectiveDescriptionText;
    [SerializeField] private ObjectiveSO noAvailableObjectiveSO;
    
    [Header("Log Data")]
    [SerializeField] private ObjectiveLogSlot[] objectiveLogSlots;
    [SerializeField] private QuestObjectiveSlot[] questObjectiveSlots;
    [SerializeField] private ObjectiveRewardSlot[] objectiveRewardSlots;
    [SerializeField] private ObjectiveManager objectiveManager;
    private ObjectiveSO objectiveSO;

    // Subscribes to objective related events 
    private void OnEnable()
    {
        ObjectiveEvents.OnObjectiveOfferRequested += ShowObjectiveOffer;    
        ObjectiveEvents.OnObjectiveTurnInRequested += ShowObjectiveTurnIn;    
    }

    // Unsubscribes from objective related events
    private void OnDisable()
    {
        ObjectiveEvents.OnObjectiveOfferRequested -= ShowObjectiveOffer;
        ObjectiveEvents.OnObjectiveTurnInRequested -= ShowObjectiveTurnIn;
    }

    // Updates and displays the current state of each quest objective in the UI slots.
    private void DisplayObjective()
    {
        for (int i = 0; i < questObjectiveSlots.Length; i++)
        {
            if (i < objectiveSO.objectives.Count)
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

    // Updates the UI to show the available objectives and setting the buttons in relation to them and their status
    public void ShowObjectiveOffer(ObjectiveSO incomingObjectiveSO)
    {
        SwitchToUIMap();
        StartCoroutine(FocusFirstSlot());

        objectiveSO = incomingObjectiveSO;

        if (objectiveManager.IsObjectiveAccepted(incomingObjectiveSO) || objectiveManager.GetCompletedObjectives(incomingObjectiveSO))
        {
            SetCanvasState(acceptCanvasGroup, false);
            SetCanvasState(completeCanvasGroup, false);
        }
        else
        {
            SetCanvasState(acceptCanvasGroup, true);
            SetCanvasState(completeCanvasGroup, false);
        }

        HandleObjectiveSelected(objectiveSO);
        SetCanvasState(objectiveCanvasGroup, true);
    }

    // Updates the UI to show the available objectives with a focus on completing those objectives
    public void ShowObjectiveTurnIn(ObjectiveSO incomingObjectiveSO)
    {
        objectiveSO = incomingObjectiveSO;
        HandleObjectiveSelected(objectiveSO);
        SetCanvasState(completeCanvasGroup, true);
        SetCanvasState(acceptCanvasGroup, false);
        SetCanvasState(objectiveCanvasGroup, true);
        SwitchToUIMap();
        StartCoroutine(FocusFirstSlot());
    }

    // Handles the logic required when an objective is accepted by the player.
    public void OnAcceptObjectiveSelected()
    {
        ObjectiveEvents.OnObjectiveAccepted?.Invoke(objectiveSO);
        objectiveManager.AcceptObjective(objectiveSO);
        SetCanvasState(completeCanvasGroup, false);
        SetCanvasState(acceptCanvasGroup, false);
        RefreshObjectiveList();
        HandleObjectiveSelected(noAvailableObjectiveSO);
    }

    // Handles the logic required when an objective is completed and turned in
    public void OnCompleteObjectiveClicked()
    {
        objectiveManager.CompleteObjective(objectiveSO);
        RefreshObjectiveList();
        HandleObjectiveSelected(noAvailableObjectiveSO);
        SetCanvasState(completeCanvasGroup, false);
    }

    // Updates the reward display slots to show the current rewards for the objective.
    private void DisplayRewards()
    {
        for (int i = 0; i < objectiveRewardSlots.Length; i++)
        {
            if (i < objectiveSO.rewards.Count)
            {
                var reward = objectiveSO.rewards[i];
                objectiveRewardSlots[i].DisplayReward(reward.itemSO.icon, reward.quantity);
                objectiveRewardSlots[i].gameObject.SetActive(true);
            }
            else
            {
                objectiveRewardSlots[i].gameObject.SetActive(false);
            }
        }
    }

    // Refreshes the objective Log with active objectives and clears the slot if there are none.
    public void RefreshObjectiveList()
    {
        List<ObjectiveSO> activeObjectives = objectiveManager.GetActiveObjectives();

        for (int i = 0; i < objectiveLogSlots.Length; i++)
        {
            if(i < activeObjectives.Count)
            {
                objectiveLogSlots[i].SetObjective(activeObjectives[i]);
            }
            else
            {
                objectiveLogSlots[i].ClearSlot();
            }
        }
    }

    // Handles the selection of objectives in the objective log displaying the name and description
    public void HandleObjectiveSelected(ObjectiveSO objectiveSO)
    {
        this.objectiveSO = objectiveSO;
        Debug.Log($"Selected Objective: {objectiveSO.objectiveName}");
        objectiveNameText.text = objectiveSO.objectiveName;
        objectiveDescriptionText.text = objectiveSO.objectiveDescription;

        DisplayObjective();
        DisplayRewards();
    }

    // Close the objective log when the close button is selected
    public void CloseObjectiveLog()
    {
        SetCanvasState(objectiveCanvasGroup, false);
        SwitchToPlayerMap();
    }

    // Sets the different canvases to either active or inactive 
    private void SetCanvasState(CanvasGroup group, bool activate)
    {
        group.alpha = activate ? 1 : 0;
        group.blocksRaycasts = activate;
        group.interactable = activate;
    }

    // Highlights the first slot provided when the objective log is opened
    private IEnumerator FocusFirstSlot()
    {
        yield return new WaitForEndOfFrame();

        if(objectiveLogSlots != null && objectiveLogSlots.Length > 0 && objectiveLogSlots[0]!= null)
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(objectiveLogSlots[0].gameObject);
        }
    }

    // Switches the player input from Player to UI so that the player can interact with the objective log UI
    private void SwitchToUIMap()
    {
        if(playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("UI");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Switches the player input back to Player from UI so that the player can resume gameplay after
    // interacting with the objective log UI
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
