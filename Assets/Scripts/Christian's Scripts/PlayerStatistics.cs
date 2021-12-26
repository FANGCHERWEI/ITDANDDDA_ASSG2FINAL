using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script contains the variables for any player statistics and calculations related to player statistics.

public class PlayerStatistics : MonoBehaviour
{
    // Below variables contain the points that the user will have for each level respectively.
    public static float PercentagePointsLvl01;
    public static float PercentagePointsLvl02;
    public static float avgPercentagePoints; // Average percentage points over all levels.

    // Below array contains the grade names in order from highest to lowest grade.
    public string[] gradeNames;

    public string grade; // Text grade of a student.
    public static int currentLvl; // Current level the student is in.

    // The below variables set the default values for their respective variables.
    private float startingPercentagePointsLvl01 = 100f;
    private float startingPercentagePointsLvl02 = 100f;

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
        avgPercentagePoints = (PercentagePointsLvl01 + PercentagePointsLvl02) / 2;

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
        // Assigns values to variables so that they are not empty and have the necessary values for the script to run.

        currentLvl = 2; // For testing only!!! The current level should only be based on buttons presses or UI elements.

        // Bottom two if statements make sure the player always begins with points to lose.
        if (PercentagePointsLvl01 <= 0f)
        {
            PercentagePointsLvl01 = startingPercentagePointsLvl01;
        }

        if (PercentagePointsLvl02 <= 0f)
        {
            PercentagePointsLvl02 = startingPercentagePointsLvl02;
        }
    }
}
