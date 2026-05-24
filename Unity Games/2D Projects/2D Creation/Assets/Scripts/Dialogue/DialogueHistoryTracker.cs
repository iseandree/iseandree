using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHistoryTracker : MonoBehaviour
{
    private readonly List<ActorSO> spokenNPCs = new List<ActorSO>();

    public void RecordNPC(ActorSO actorSO)
    {
        spokenNPCs.Add(actorSO);
        Debug.Log("Just spoke to " + actorSO.actorName);
    }

    public bool HasSpokenWith(ActorSO actorSO)
    {
        return spokenNPCs.Contains(actorSO);
    }
}
