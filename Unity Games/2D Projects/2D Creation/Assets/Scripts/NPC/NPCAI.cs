using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;



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
    [SerializeField] private float circleCastRadius = 2.0f;
    [SerializeField] private bool hasQuest = false;
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
    private Animator animator;
    private GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        randomTime = Random.Range(minWalkTime, maxWalkTime);
    }

    private void OnEnable()
    {
        ObjectiveEvents.OnObjectiveAccepted += OnObjectiveAccepted_RemoveOfferings;
    }

    private void OnDisable()
    {
        ObjectiveEvents.OnObjectiveAccepted -= OnObjectiveAccepted_RemoveOfferings;
    }

    // Update is called once per frame
    void Update()
    {
        bool isPlayerNearby = (hasQuest || hasReasonToTalk) && DetectPlayer();

        if (isTalking && !GameManager.Instance.dialogueManager.isDialogueActive)
        {
            isTalking = false;
        }
        
        if (isTalking || isPlayerNearby)
        {
            isWalking = false;
            rb.linearVelocity = Vector2.zero;
            if(isPlayerNearby)
            { 
                interactAnim.Play("Open Icon"); 
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= randomTime)
            {
                StateChange();
            }
            if (!isFlipping && (transform.position.x < leftPatrolX || transform.position.x > rightPatrolX))
            {
                StartCoroutine(Flip());
            }

            if (isWalking)
            {
                rb.linearVelocity = Vector2.right * facingDirection * moveSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        if ((hasQuest || hasReasonToTalk) && player != null)
        {
            PlayerInput playerInput = player.GetComponent<PlayerInput>();
            if (playerInput != null && playerInput.actions != null)
            {
                InputAction interactAction = playerInput.actions.FindAction("Interact");
                if (interactAction != null && interactAction.WasPressedThisFrame())
                {
                    Debug.Log("[NPC] Interact button pressed!");
                    if (GameManager.Instance.dialogueManager.isDialogueActive)
                    {
                        Debug.Log("[NPC] Dialogue is active, advancing...");
                        GameManager.Instance.dialogueManager.AdvanceDialogue();
                    }
                    else
                    {
                        if (GameManager.Instance.dialogueManager.CanStartDialogue())
                        {
                            Debug.Log("[NPC] Starting a fresh conversation...");
                            CheckForNewConversation();
                            Debug.Log($"[NPC] Current Conversation Asset: {currentConversation?.name}");
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

        animator.SetBool("isWalking", isWalking);
    }

    private void CheckForNewConversation()
    {
        Debug.Log($"[NPC Debug] Checking conversations. Total count available: {conversations.Count}");
        for (int i = 0; i < conversations.Count; i++)
        {
            var convo = conversations[i];
            if (convo != null && convo.IsConditionMet())
            {
                currentConversation = convo;
                Debug.Log($"[NPC Debug] Evaluating conditions for: {convo.name}");
                //Remove if its one time only
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

    IEnumerator Flip()
    {
        isFlipping = true;
        transform.Rotate(0, 180, 0);
        facingDirection *= -1;
        yield return new WaitForSeconds(0.5f);
        isFlipping = false;
    }

    /// <summary>
    /// Determines whether an interactable object is within range and updates the current interactable reference
    /// accordingly.
    /// </summary>
    /// <remarks>This method searches for the closest collider tagged as "Interactable" within a specified
    /// radius of the player. If found, the interactable reference is updated to the corresponding GameObject;
    /// otherwise, it is set to null.</remarks>
    /// <returns>true if a valid interactable object is detected within range; otherwise, false.</returns>
    private bool DetectPlayer()
    {
        // Get an array of all possible colliders within the range of the NPC
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCastRadius);

        // Check if there are any colliders within the range of the NPC. If not close the method
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
         A) They have the required Tag and B) Which ideal interactable is closest to the player */
        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Player"))
            {
                continue;
            }

            float distanceFromNPC = Vector2.Distance(transform.position, hit.transform.position);
            if (distanceFromNPC < distanceFromMatching)
            {
                // Store the ideal Collider as the confirmed matching collider and store its distance from the player
                matchingPlayer = hit;
                distanceFromMatching = distanceFromNPC;
            }
        }

        // Check if the ideal Collider has been assigned a match, if not close the method
        if (matchingPlayer == null)
        {
            player = null;
            interactAnim.Play("Close Icon");
            return false;
        }

        // Store the game object attached to the ideal Collider to the confirmed interactable and return true for this method. 
        player = matchingPlayer.gameObject;
        return true;
    }

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
}
