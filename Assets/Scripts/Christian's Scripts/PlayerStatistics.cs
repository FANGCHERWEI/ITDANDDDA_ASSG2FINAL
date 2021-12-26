using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script contains the variables for any player statistics and calculations related to player statistics.

public class PlayerStatistics : MonoBehaviour
{
    public float maxPercentagePointsLvl01;
    public float maxPercentagePointsLvl02;
    public float avgPercentagePoints;
    public string grade;

    public int currentLvl;

    private void Awake()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckForGrade();
    }

    private void CheckForGrade()
    {
        avgPercentagePoints = (maxPercentagePointsLvl01 + maxPercentagePointsLvl02) / 2;

        if (avgPercentagePoints > 80)
        {
            grade = "Excellent";
        }

        else if (avgPercentagePoints > 50)
        {
            grade = "Good";
        }

        else
        {
            grade = "Epic Fail";
        }
    }

    private void InitializeVariables()
    {
        currentLvl = 1; // For testing only!!! The current level should only be based on buttons presses or UI elements.

        if (maxPercentagePointsLvl01 <= 0f)
        {
            maxPercentagePointsLvl01 = 100f;
        }

        if (maxPercentagePointsLvl02 <= 0f)
        {
            maxPercentagePointsLvl02 = 100f;
        }
    }
}
