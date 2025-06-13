using System;
using UnityEngine;

/// <summary>
/// EventManager is responsible for managing events related to the basketball game. It provides static methods to trigger events for picking up, releasing, and throwing the ball.
/// </summary>
public class EventManager : MonoBehaviour
{
    // Ball-Related Events
    public static Action<Vector2, float> OnPickUp;
    public static Action<Vector2, Vector2, float> OnRelease;
    public static Action<Vector2, Vector2, float> OnThrow;
    public static Action OnBallThrown;

    // Ball-Related Methods
    public static void PickUpBall(Vector2 position, float time) => OnPickUp?.Invoke(position, time);   
    public static void DropBall(Vector2 position, Vector2 dropPosition, float time) => OnRelease?.Invoke(position, dropPosition, time);
    public static void ThrowBall(Vector2 start, Vector2 end, float time) => OnThrow?.Invoke(start, end, time);
    public static void BallThrown() => OnBallThrown?.Invoke();
}
