using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ObjectiveLogUI : MonoBehaviour
{
    [Header("Setup References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CanvasGroup objectiveCanvasGroup;
    [SerializeField] private CanvasGroup acceptCanvasGroup;
    [SerializeField] private CanvasGroup declineCanvasGroup;
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

    private void OnEnable()
    {
        ObjectiveEvents.OnObjectiveOfferRequested += ShowObjectiveOffer;    
        ObjectiveEvents.OnObjectiveTurnInRequested += ShowObjectiveTurnIn;    
    }
    private void OnDisable()
    {
        ObjectiveEvents.OnObjectiveOfferRequested -= ShowObjectiveOffer;
        ObjectiveEvents.OnObjectiveTurnInRequested -= ShowObjectiveTurnIn;
    }


    // Only doing this because of the video but there will be no board to get quests from 
    public void ShowObjectiveOffer(ObjectiveSO incomingObjectiveSO)
    {
        SwitchToUIMap();
        StartCoroutine(FocusFirstSlot());

        objectiveSO = incomingObjectiveSO;

        if (objectiveManager.IsObjectiveAccepted(incomingObjectiveSO) || objectiveManager.GetCompletedObjectives(incomingObjectiveSO))
        {
            SetCanvasState(acceptCanvasGroup, false);
            SetCanvasState(declineCanvasGroup, true);
            SetCanvasState(completeCanvasGroup, false);
        }
        else
        {
            SetCanvasState(acceptCanvasGroup, true);
            SetCanvasState(declineCanvasGroup, true);
            SetCanvasState(completeCanvasGroup, false);
        }
        HandleQuestSelected(objectiveSO);
        SetCanvasState(objectiveCanvasGroup, true);
    }

    public void ShowObjectiveTurnIn(ObjectiveSO incomingObjectiveSO)
    {
        objectiveSO = incomingObjectiveSO;
        HandleQuestSelected(objectiveSO);
        SetCanvasState(completeCanvasGroup, true);
        SetCanvasState(acceptCanvasGroup, false);
        SetCanvasState(declineCanvasGroup, false);
        SetCanvasState(objectiveCanvasGroup, true);
        SwitchToUIMap();
        StartCoroutine(FocusFirstSlot());
    }

    public void OnAcceptObjectiveSelected()
    {
        objectiveManager.AcceptObjective(objectiveSO);

        SetCanvasState(completeCanvasGroup, false);
        SetCanvasState(acceptCanvasGroup, false);
        SetCanvasState(declineCanvasGroup, false);
        RefreshObjectiveList();
        HandleQuestSelected(noAvailableObjectiveSO);

    }

    public void OnDeclineObjectiveSelected()
    {
        SetCanvasState(objectiveCanvasGroup, false);
        SwitchToPlayerMap();
    }

    public void OnCompleteObjectiveClicked()
    {
        objectiveManager.CompleteObjective(objectiveSO);
        RefreshObjectiveList();
        HandleQuestSelected(noAvailableObjectiveSO);
        SetCanvasState(completeCanvasGroup, false);
    }

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

    public void HandleQuestSelected(ObjectiveSO objectiveSO)
    {
        this.objectiveSO = objectiveSO;
        Debug.Log($"Selected Objective: {objectiveSO.objectiveName}");
        objectiveNameText.text = objectiveSO.objectiveName;
        objectiveDescriptionText.text = objectiveSO.objectiveDescription;

        DisplayObjective();
        DisplayRewards();
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

    private void SetCanvasState(CanvasGroup group, bool activate)
    {
        group.alpha = activate ? 1 : 0;
        group.blocksRaycasts = activate;
        group.interactable = activate;
    }
}
