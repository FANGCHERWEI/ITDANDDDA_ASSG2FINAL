using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public float maxPercentagePointsLvl01;
    public float maxPercentagePointsLvl02;
    public float maxPercentagePointsLvl03;
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
        avgPercentagePoints = (maxPercentagePointsLvl01 + maxPercentagePointsLvl02 + maxPercentagePointsLvl03) / 3;

        if (avgPercentagePoints > 80)
            grade = "Excellent";

        else if (avgPercentagePoints > 50)
            grade = "Good";

        else
            grade = "Epic Fail";
    }

    private void InitializeVariables()
    {
        currentLvl = 1;

        if (maxPercentagePointsLvl01 <= 0f)
            maxPercentagePointsLvl01 = 100f;

        if (maxPercentagePointsLvl02 <= 0f)
            maxPercentagePointsLvl02 = 100f;

        if (maxPercentagePointsLvl03 <= 0f)
            maxPercentagePointsLvl03 = 100f;
    }
}
