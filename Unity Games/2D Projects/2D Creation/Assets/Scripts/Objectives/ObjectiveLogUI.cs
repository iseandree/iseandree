using System;
using System.Collections;
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

    [SerializeField] private ObjectiveManager objectiveManager;
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
        Debug.Log($"Selected Objective: {objectiveSO.objectiveName}");

        foreach (var objective in objectiveSO.objectives)
        {
            objectiveManager.UpdateObjectiveProgress(objectiveSO, objective);
            Debug.Log($"Objective: {objective.description}");
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
