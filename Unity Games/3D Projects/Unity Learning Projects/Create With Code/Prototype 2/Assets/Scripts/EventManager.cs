using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<int> OnScoreUpdated;
    public static event Action<int> OnLifeLost;

    public static void ScoreUpdated(int score) => OnScoreUpdated?.Invoke(score);
    public static void LifeLost(int life) => OnLifeLost?.Invoke(life);

}
