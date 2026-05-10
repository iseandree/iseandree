using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Day/Night Cycle Referenced by Mina Pêcheux from https://medium.com/codex/creating-a-basic-day-and-night-cycle-in-unity-c-dff942c1690d
public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct DayAndNightTimeStamp
    {
        public float timeRatio;
        public Color color;
        public float intensity;
    }

    private enum DifficultySettings {Easy, Normal, Hard};
    [SerializeField] private DifficultySettings difficulty;

    // SerializeField - Day/Night
    [SerializeField] DayAndNightTimeStamp[] timeStamps;
    [SerializeField] float cycleLength; // the total duration of the cycle (from morning to morning) in seconds
    [SerializeField] Light2D light2D;

    // Private Variables - To compare to the current cycle time variable, acknowledging if a time stamp has passed or not.
    private float countDown = 180.0f;
    private float difficultyModifier;
    private float timeStampDifference;
    private float currentCycleTime; // the current time that has elapsed in this cycle
    private float currentTimeStamp; // the exact time in seconds for the current time stamp
    private float nextTimeStamp; // the exact time in seconds for the next time stamp
    private int currentTimeStampIndex;   // index for the current time stamp
    private int nextTimeStampIndex; // index for the upcoming time stamp
    private bool isGameRunning;

    // Temporary Setup
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI timerText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DifficultySelect();
        isGameRunning = true;
        cycleLength = countDown * difficultyModifier;
        currentTimeStampIndex = -1;
        CycleTimeStamps();
    }

   

    // Update is called once per frame
    void Update()
    {
        if(!isGameRunning)
        {
            return;
        }

        // Count the total time elapsed since the game has started and have it wrap back to 0 when the cycle ends using modulo
        currentCycleTime = (currentCycleTime + Time.deltaTime) % cycleLength;

        // Blend color and intensity between timestamps
        float lerpTime = (currentCycleTime - currentTimeStamp) / timeStampDifference;
        DayAndNightTimeStamp current = timeStamps[currentTimeStampIndex];
        DayAndNightTimeStamp next = timeStamps[nextTimeStampIndex];
        light2D.color = Color.Lerp(current.color, next.color, lerpTime);
        light2D.intensity = Mathf.Lerp(current.intensity, next.intensity, lerpTime);

        // Check if a time stamp has passed, compare the updated currentCycleTime with the time in seconds of the next time stamp
        if(currentCycleTime >= nextTimeStamp && currentTimeStampIndex < timeStamps.Length - 1)
        {
            light2D.color = next.color;
            light2D.intensity = next.intensity;
            CycleTimeStamps();
        }
        else if (currentCycleTime >= cycleLength)
        {
            isGameRunning = false;
        }

        timerText.text = currentCycleTime.ToString();
    }

    private void CycleTimeStamps()
    {
        // increment the currentTimeStampIndex, but with a modulo operation to ensure it doesn’t overshoot the length of the marks array
        currentTimeStampIndex = (currentTimeStampIndex + 1) % timeStamps.Length; // should be index 0 to start then increase
        nextTimeStampIndex = (currentTimeStampIndex + 1) % timeStamps.Length; // should be index 1 to start then increase

        /* By multiplying the normalised time ratio of the time stamp by the total length of the cycle the indices can
        get back to the actual DayAndNightTimeStamp objects from the array and store their actual time in the cycle in seconds */
        currentTimeStamp = timeStamps[currentTimeStampIndex].timeRatio * cycleLength;
        nextTimeStamp = timeStamps[nextTimeStampIndex].timeRatio * cycleLength;

        // The total duration between the current and next time stamps
        timeStampDifference = nextTimeStamp - currentTimeStamp; // The difference between the next and the current time stamps

        // Reached the last mark in the cycle re-add the cycle length to this duration to get back a positive number
        if (timeStampDifference < 0)
        {
            timeStampDifference += cycleLength; 
        }
    }

    void DifficultySelect()
    {
        switch(difficulty)
        {
            case DifficultySettings.Easy:
                difficultyModifier = 1.5f;
                break;
            case DifficultySettings.Normal:
                difficultyModifier = 1.0f;
                break;
            case DifficultySettings.Hard:
                difficultyModifier = 0.5f;
                break;
            default:
                break;
        }
    }
}
