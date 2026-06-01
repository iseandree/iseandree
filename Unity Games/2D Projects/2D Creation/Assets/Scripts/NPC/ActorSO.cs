using UnityEngine;

// Actor Scriptable Object to give names/identities to NPCs 
[CreateAssetMenu(fileName = " ActorSO", menuName = "Dialogue/NPC")]
public class ActorSO : ScriptableObject
{
    public string actorName;
}
