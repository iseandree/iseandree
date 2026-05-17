using System;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public GameObjectiveScriptableObject objectiveSO;
    public bool isCompleted;
    public bool isAccepted;
    public static event Action<GameObjectiveScriptableObject, bool> OnObjectiveAccepted;

    /// <summary>
    /// Performs validation logic when the component's properties are changed in the editor.
    /// </summary>
    /// <remarks>This method is called automatically by the Unity Editor when a value on the component is
    /// modified. Override this method to implement custom validation or to enforce constraints on serialized fields
    /// during editing.</remarks>
    private void OnValidate()
    {
        if (objectiveSO == null)
        {
            return;
        }
        
        this.name = objectiveSO.objectiveName;
    }

    public void AcceptQuest()
    {
        OnObjectiveAccepted?.Invoke(objectiveSO, isAccepted);
    }
}
