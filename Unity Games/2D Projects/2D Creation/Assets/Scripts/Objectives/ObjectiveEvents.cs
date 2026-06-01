using System;
using UnityEngine;

// Provides events and delegates related to objective lifecycle actions, such as offering, accepting, turning in, and
// checking completion status of objectives.
public static class ObjectiveEvents 
{
    public static Action<ObjectiveSO> OnObjectiveOfferRequested;
    public static Action<ObjectiveSO> OnObjectiveTurnInRequested;
    public static Action<ObjectiveSO> OnObjectiveAccepted;
    public static Func<ObjectiveSO, bool> IsObjectiveCompleted;
}
