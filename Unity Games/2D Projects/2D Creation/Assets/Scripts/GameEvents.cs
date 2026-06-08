using System;
using UnityEngine;

// Provides events and members for game setup from the main menu
public static class GameEvents
{
    public static int SavedDifficulty = 0;
    public static Action<int> OnDifficultySelected;

    public static void RaiseDifficultySelected(int difficulty)
    {
        SavedDifficulty = difficulty;
        OnDifficultySelected?.Invoke(difficulty);
    }
}
