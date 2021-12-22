using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public float maxPercentagePoints;
    public string grade;

    private void Start()
    {
        if (maxPercentagePoints <= 0f)
            maxPercentagePoints = 100f;
    }

    private void Update()
    {
        CheckForGrade();
    }

    private void CheckForGrade()
    {
        if (maxPercentagePoints > 80)
            grade = "Excellent";

        else if (maxPercentagePoints > 50)
            grade = "Good";

        else
            grade = "Epic Fail";
    }
}
