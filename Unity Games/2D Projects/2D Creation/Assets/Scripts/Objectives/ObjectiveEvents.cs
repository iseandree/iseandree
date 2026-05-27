using System;
using UnityEngine;

public static class ObjectiveEvents 
{
    public static Action<ObjectiveSO> OnObjectiveOfferRequested;
    public static Action<ObjectiveSO> OnObjectiveTurnInRequested;
    public static Action<ObjectiveSO> OnObjectiveAccepted;
    public static Func<ObjectiveSO, bool> IsObjectiveCompleted;
}
