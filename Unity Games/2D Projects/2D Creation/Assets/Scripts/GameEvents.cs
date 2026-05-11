using System;
using UnityEngine;

public static class GameEvents
{
    public static int SavedDifficulty = 1;
    public static Action<int> OnDifficultySelected;

    public static void RaiseDifficultySelected(int difficulty)
    {
        SavedDifficulty = difficulty;
        OnDifficultySelected?.Invoke(difficulty);
    }
}
