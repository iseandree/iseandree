using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

// Tracks the dialogue that has been accessed by the player within their playthrough - Should probably save this info
public class DialogueHistoryTracker : MonoBehaviour
{
    // Private Variables
    private readonly List<ActorSO> spokenNPCs = new List<ActorSO>();

    // Make not of any NPC acknowledged and store them in this list for future reference
    public void RecordNPC(ActorSO actorSO)
    {
        spokenNPCs.Add(actorSO);
    }

    // Check if the player has spoken to certain NPCs - for Objectives
    public bool HasSpokenWith(ActorSO actorSO)
    {
        return spokenNPCs.Contains(actorSO);
    }
}
