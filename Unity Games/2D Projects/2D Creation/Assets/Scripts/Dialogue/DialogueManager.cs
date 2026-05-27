using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [Header("UI References")]
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public Button[] choiceButtons;
    public CanvasGroup canvasGroup;
    [SerializeField] private PlayerInput playerInput;
    private string uiAdvanceActionName = "Submit";

    public bool isDialogueActive;
    private bool areChoicesShowing = false;
    private bool skipFrameInput = false;

    private DialogueSO currentDialogue;
    private int dialogueIndex;

    private float dialogueCooldown = 0.1f;
    private float lastDialogueEndTime;

    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        ResetButtons();
    }

    private void Update()
    {
        if (skipFrameInput)
        {
            skipFrameInput = false;
            return;
        }

        if (isDialogueActive && !areChoicesShowing)
        {
            if (playerInput != null && playerInput.actions != null)
            {
                InputAction advanceAction = playerInput.actions.FindAction(uiAdvanceActionName);

                // Read input directly from the currently active UI action map
                if (advanceAction != null && advanceAction.WasPressedThisFrame())
                {
                    AdvanceDialogue();
                }
            }
        }
    }

    public bool CanStartDialogue()
    {
        return Time.unscaledTime - lastDialogueEndTime >= dialogueCooldown;
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {
        currentDialogue = dialogueSO;
        dialogueIndex = 0;
        isDialogueActive = true;
        areChoicesShowing = false;
        SwitchToUIMap();
        ResetButtons();
        ShowDialogue();
        skipFrameInput = true;
    }


    public void AdvanceDialogue()
    {
        if (dialogueIndex < currentDialogue.lines.Length)
        {
            ShowDialogue();
        }
        else
        {
            ShowChoices();
        }

    }    
    private void ShowChoices()
    {
        ResetButtons();
        areChoicesShowing = true;

        if (currentDialogue.options.Length > 0)
        {
            for(int i = 0; i < currentDialogue.options.Length; i++)
            {
                if (i >= choiceButtons.Length) break;

                var option = currentDialogue.options[i];
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = option.optionText;
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].onClick.AddListener(() => ChooseOption(option.nextDialogue));
            }
            StartCoroutine(FocusFirstButton());
        }
        else
        {
            if(currentDialogue.turnInObjectiveOnEnd != null && 
                GameManager.Instance.objectiveManager.IsObjectiveComplete(currentDialogue.turnInObjectiveOnEnd))
            {
                EndDialogue();
                ObjectiveEvents.OnObjectiveTurnInRequested?.Invoke(currentDialogue.turnInObjectiveOnEnd);
                
            }
            else if (currentDialogue.offerObjectiveOnEnd != null)
            {
                EndDialogue();
                ObjectiveEvents.OnObjectiveOfferRequested?.Invoke(currentDialogue.offerObjectiveOnEnd);
            }
            else
            {
                choiceButtons[0].GetComponentInChildren<TMP_Text>().text = "Ok.";
                choiceButtons[0].onClick.AddListener(EndDialogue);
                choiceButtons[0].gameObject.SetActive(true);

                StartCoroutine(FocusFirstButton());
            }
        }
    }

    private void ChooseOption(DialogueSO dialogueSO)
    {
        if(dialogueSO == null)
        {
            EndDialogue();
        }
        else
        {
            ResetButtons();
            StartDialogue(dialogueSO);
        }
    }

    private void EndDialogue()
    {
        dialogueIndex = 0;
        isDialogueActive = false;
        areChoicesShowing = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        SwitchToPlayerMap();
        ResetButtons();
    }

    private void ShowDialogue()
    {
        DialogueLine line = currentDialogue.lines[dialogueIndex];
        GameManager.Instance.dialogueHistoryTracker.RecordNPC(line.speaker);

        actorName.text = line.speaker.actorName;
        dialogueText.text = line.text;
        dialogueIndex++;

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private void ResetButtons()
    {
        foreach (var button in choiceButtons)
        {
            button.onClick.RemoveAllListeners(); // Crucial! Stops multiple inputs firing at once
            button.gameObject.SetActive(false);
        }
    }

    private IEnumerator FocusFirstButton()
    {
        yield return new WaitForEndOfFrame();
        if (choiceButtons.Length > 0 && choiceButtons[0] != null)
        {
            // Clear current selection first to ensure Unity registers the change
            EventSystem.current.SetSelectedGameObject(null);

            // Set selection to the first choice button
            EventSystem.current.SetSelectedGameObject(choiceButtons[0].gameObject);
        }
    }

    private void SwitchToUIMap()
    {
        if (playerInput != null)
        {
            // Changes the active map to "UI". Replace "UI" with your exact UI map name.
            playerInput.SwitchCurrentActionMap("UI");

            // Optional: Unlock cursor for MnK players if your menu allows mouse clicking
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void SwitchToPlayerMap()
    {
        if (playerInput != null)
        {
            // Changes the active map back to gameplay. Replace "Player" with your exact player map name.
            playerInput.SwitchCurrentActionMap("Player");

            // Optional: Re-lock cursor for gameplay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
