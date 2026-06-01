using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/* Manages the display and flow of interactive dialogue sequences, including presenting dialogue lines, handling player
choices, and coordinating UI state transitions. 
Code inspired/sourced by Night Run Studio https://www.youtube.com/playlist?list=PLSR2vNOypvs6CNsu9fYk2v9DOZOTfSHPc */
public class DialogueManager : MonoBehaviour
{
    // Public variables
    [Header("UI References")]
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public Button[] choiceButtons;
    public CanvasGroup canvasGroup;
    public bool isDialogueActive;

    // Private Variables
    [SerializeField] private PlayerInput playerInput;
    private bool areChoicesShowing = false;
    private bool skipFrameInput = false;
    private DialogueSO currentDialogue;
    private int dialogueIndex;
    private float dialogueCooldown = 0.1f;
    private float lastDialogueEndTime;

    // When the game starts set the Dialogue UI to essentially inactive
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
                InputAction advanceAction = playerInput.actions.FindAction("Submit");

                // Read input directly from the currently active UI action map
                if (advanceAction != null && advanceAction.WasPressedThisFrame())
                {
                    AdvanceDialogue();
                }
            }
        }
    }

    // Determines whether a new dialogue can be started by enforcing a minimum interval between them
    public bool CanStartDialogue()
    {
        return Time.unscaledTime - lastDialogueEndTime >= dialogueCooldown;
    }

    // When the player interacts with NPCs or objects that provide dialogue switch to the UI map to navigate the UI
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

    // Begins a new dialogue sequence using the specified dialogue script object.
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

    // Displays the current dialogue line in the user interface and updates the dialogue state.
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

    // Iterates throught the lines of dialogue 
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

    // Ends the current dialogue session and resets the dialogue UI to its default state.
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

    // Displays the available dialogue choices to the user and configures the choice buttons based on the current
    // dialogue state.
    private void ShowChoices()
    {
        // Safety check the buttons first
        ResetButtons();
        areChoicesShowing = true;

        // If there are choices in the dialogue configure the buttons to show those choices
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
        else    // If not, go through ways to end the conversation
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

    // Selects and initiates a dialogue sequence based on the specified dialogue script object.
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

    // Sets keyboard focus to the first available choice button after the current frame has finished rendering.
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
 
    // When the player is done using the UI system switch back to regular player input
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

    // Resets the buttons after use to prevent lingering inputs.
    private void ResetButtons()
    {
        foreach (var button in choiceButtons)
        {
            button.onClick.RemoveAllListeners(); // Crucial! Stops multiple inputs firing at once
            button.gameObject.SetActive(false);
        }
    }
}
