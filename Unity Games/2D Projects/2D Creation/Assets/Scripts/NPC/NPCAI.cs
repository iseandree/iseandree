using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

/* Controls the behavior and interactions of a non-player character (NPC) in the game, including patrolling, detecting
the player, and managing dialogue or quest-related interactions. 
Code inspired/sourced by Night Run Studio https://www.youtube.com/playlist?list=PLSR2vNOypvs6CNsu9fYk2v9DOZOTfSHPc
https://www.youtube.com/watch?v=bj_tJMiUut0&list=PLSR2vNOypvs7XBLwVr0rb9WHeoNuEotd0&index=5&t=1s */
public class NPCAI : MonoBehaviour
{
    // Serialize Field
    [SerializeField] private float leftPatrolX;
    [SerializeField] private float rightPatrolX;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float minPauseTime;
    [SerializeField] private float maxPauseTime;
    [SerializeField] private float minWalkTime;
    [SerializeField] private float maxWalkTime;
    [SerializeField] private Animator interactAnim;
    [SerializeField] private GameObject aura;
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private float circleCastRadius = 2.0f;
    [SerializeField] private bool hasObjective = false;
    [SerializeField] private bool hasReasonToTalk = false;
    [SerializeField] private DialogueSO currentConversation;
    [SerializeField] private List<DialogueSO> conversations;

    // Private Variables
    private Rigidbody2D rb;
    private int facingDirection = -1;
    private float randomTime;
    private float timer;
    private bool isWalking = true;
    private bool isFlipping = false;
    private bool isTalking = false;
    private bool isPlayerNearby = false;
    private Animator animator;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        randomTime = Random.Range(minWalkTime, maxWalkTime);
        interactIcon.SetActive(false);
        aura.SetActive(false);
    }

    // When scene starts subscribe to Objective Event OnObjectiveAccepted
    private void OnEnable()
    {
        ObjectiveEvents.OnObjectiveAccepted += OnObjectiveAccepted_RemoveOfferings;
    }

    // When scene ends unsubscribe to Objective Event OnObjectiveAccepted
    private void OnDisable()
    {
        ObjectiveEvents.OnObjectiveAccepted -= OnObjectiveAccepted_RemoveOfferings;
    }

    // Update is called once per frame
    void Update()
    {
        // Variable to use if the player is near and this particular npc has a reason to talk to the player 
        if (hasObjective || hasReasonToTalk)
        {
            isPlayerNearby = DetectPlayer();
        }

        // If this NPC is already talking but there is no dialogue active reset their talking state
        if (isTalking && !GameManager.Instance.dialogueManager.isDialogueActive)
        {
            isTalking = false;
        }

        UpdatePatrolAndPlayerProximity(isPlayerNearby);

        TryInteractWithPlayer();

        // Animate this NPC walking if they are supposed to be walking
        animator.SetBool("isWalking", isWalking);

    }

    // This changes the states in which this NPC will operate under randomly as they wander
    private void StateChange()
    {
        isWalking = !isWalking;
        if (isWalking)
        {
            randomTime = Random.Range(minWalkTime, maxWalkTime);
        }
        else
        {
            randomTime = Random.Range(minPauseTime, maxPauseTime);
        }
        timer = 0;
    }

    // This flips this NPC to face the direction they are heading toward
    IEnumerator Flip()
    {
        isFlipping = true;
        transform.Rotate(0, 180, 0);
        facingDirection *= -1;
        yield return new WaitForSeconds(0.5f);
        isFlipping = false;
    }

    // Determines whether an interactable object is within range and updates the current interactable reference
    // accordingly.
    private bool DetectPlayer()
    {
        // Get an array of all possible colliders within the range of this NPC
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCastRadius);

        // Check if there are any colliders within the range of this NPC. If not close the method
        if (hits.Length == 0)
        {
            player = null;
            Debug.Log("No collisions with Player detected");
            return false;
        }

        // Variables to store the ideal interactable once found
        Collider2D matchingPlayer = null;
        float distanceFromMatching = Mathf.Infinity;

        /* Loop through all possible colliders within the range of the player and determine if:
         A) They have the required Tag and B) Which ideal interactable is closest to this NPC */
        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Player"))
            {
                continue;
            }

            float distanceFromPlayer = Vector2.Distance(transform.position, hit.transform.position);
            if (distanceFromPlayer < distanceFromMatching)
            {
                // Store the ideal Collider as the confirmed matching collider and store its distance from this N{C
                matchingPlayer = hit;
                distanceFromMatching = distanceFromPlayer;
                interactIcon.SetActive(true);
            }
        }

        // Check if the ideal Collider has been assigned a match, if not close the method
        if (matchingPlayer == null)
        {
            player = null;
            interactAnim.Play("Close Icon");
            StartCoroutine(DisableAfterAnimation(interactIcon, "Close Icon"));
            return false;
        }

        // Store the game object attached to the ideal Collider to the confirmed interactable and return true for this method. 
        player = matchingPlayer.gameObject;
        return true;
    }

    // This updates the wander state and deals with this NPC's proximity to the player.
    private void UpdatePatrolAndPlayerProximity(bool isPlayerNearby)
    {
        // If this NPC is talking to the player already or if the player is nearby stop moving and play animation
        if (isTalking || isPlayerNearby)
        {
            isWalking = false;
            rb.linearVelocity = Vector2.zero;
            if (isPlayerNearby)
            {
                interactAnim.Play("Open Icon");
            }
        }
        else    // Otherwise just wander back and forth doing whatever states are avaialable 
        {
            timer += Time.deltaTime;

            // After a certain amount of random time change states
            if (timer >= randomTime)
            {
                StateChange();
            }

            // As long as this NPC is not flipping already and they have reached the bounds of their wander location start to flip them 
            if (!isFlipping && (transform.position.x < leftPatrolX || transform.position.x > rightPatrolX))
            {
                StartCoroutine(Flip());
            }

            // If this NPC is walking do it at a certain speed facing the respective direction
            if (isWalking)
            {
                rb.linearVelocity = Vector2.right * facingDirection * moveSpeed;
            }
            else // Otherwise stop moving
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    // This determines if this NPC has reason to interact with the player and if so, wait for input from the player and do so
    private void TryInteractWithPlayer()
    {
        // If this NPC has an objective for the player or a reason to talk to them and the player is in the game then interact with them
        if ((hasObjective || hasReasonToTalk) && player != null)
        {
            PlayerInput playerInput = player.GetComponent<PlayerInput>();

            // If the player has the ability to input an action and they press the interact button activate dialogue
            if (playerInput != null && playerInput.actions != null)
            {
                InputAction interactAction = playerInput.actions.FindAction("Interact");

                // As long as the interactAction is actually set in the project settings and was pressed this frame continue
                if (interactAction != null && interactAction.WasPressedThisFrame())
                {
                    // If dialogue is already active, continue through the lines
                    if (GameManager.Instance.dialogueManager.isDialogueActive)
                    {
                        GameManager.Instance.dialogueManager.AdvanceDialogue();
                    }
                    else   // If it is not active, check for and start a new conversation
                    {
                        if (GameManager.Instance.dialogueManager.CanStartDialogue())
                        {
                            CheckForNewConversation();
                            GameManager.Instance.dialogueManager.StartDialogue(currentConversation);

                            // Lock the NPC into the talking state
                            isTalking = true;
                            isWalking = false;
                            rb.linearVelocity = Vector2.zero;
                        }
                    }
                }
            }
        }
    }

    // Checks if this NPC has anything to say 
    private void CheckForNewConversation()
    {
        // Loop through the list of conversations and if there is a conversation to be had continue
        for (int i = 0; i < conversations.Count; i++)
        {
            var convo = conversations[i];
            // If there is a conversation available and the conditions for that conversation have been met set it as the current conversation
            if (convo != null && convo.IsConditionMet())
            {
                currentConversation = convo;

                // Remove if its a one time conversation
                if (convo.removeAfterPlay)
                {
                    conversations.RemoveAt(i);
                }
                
                // Remove any other dialogues that should be cleared when this one plays
                if(convo.removeTheseOnPlay != null && convo.removeTheseOnPlay.Count > 0)
                {
                    foreach(var toRemove in convo.removeTheseOnPlay)
                    {
                        conversations.Remove(toRemove);
                    }
                }

                currentConversation = convo;
                break;
            }
            else
            {
                Debug.LogWarning($"[NPC Debug] Slot {i} in conversations list is null!");
            }
        }
    }

    // Removes all conversations that offer the specified objective from the collection when the objective is accepted.
    private void OnObjectiveAccepted_RemoveOfferings(ObjectiveSO acceptedObjective)
    {
        for(int i = conversations.Count - 1; i >= 0; i--)
        {
            var convo = conversations[i];
            if(convo == null)
            {
                continue;
            }
            if(convo.offerObjectiveOnEnd == acceptedObjective)
            {
                conversations.RemoveAt(i);
            }
        }
    }

    // Wait for an animation to finish before disabling game object again
    private IEnumerator DisableAfterAnimation(GameObject target, string animName)
    {
        yield return null;

        // Get the duration of the current animation clip
        float duration = interactAnim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(duration);

        // Deactivate the object safely
        target.SetActive(false);
    }
}
