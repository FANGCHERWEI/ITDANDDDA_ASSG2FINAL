using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public PlayerStatistics playerStats;
    public ToggleGameObjects toggleGameObjects;

    // Below variables will be used to update SensorCollisionDetector.cs script variables.
    public bool allowSensorCollisionDetectorOperation;
    public int numTimesChurningCompleted;

    // Below variables will be used to update CookingDetector.cs script variables.
    public bool allowCookingDetectorOperation;
    public int numTimesCookingCompleted;

    public bool gameStart;
    public string currentStageOfProcess;

    private string[] stage01TaskNamesInOrder = { "Churning" };
    private string[] stage02TaskNamesInOrder = { "Churning", "Cooking", "Churning", "Cooking" };
    private string[] stage03TaskNamesInOrder = { "Churning", "Simmering", "Seasoning", "Churning", "Seasoning" };

    [SerializeField] private List<string[]> allTaskNamesInOrder = new List<string[]>();


    private void Awake()
    {
        gameStart = true;
        currentStageOfProcess = stage01TaskNamesInOrder[0];

        allTaskNamesInOrder.Add(stage01TaskNamesInOrder);
        allTaskNamesInOrder.Add(stage02TaskNamesInOrder);
        allTaskNamesInOrder.Add(stage03TaskNamesInOrder);

        numTimesChurningCompleted = 0;

        allowSensorCollisionDetectorOperation = false;
        allowCookingDetectorOperation = false;

        FindStep01();
    }

    private void Update()
    {
        
    }

    private void ShowTaskEndMenu(int currentLvl)
    {
        // Shows the menu after the player completes a task

        gameStart = false;

        if (currentLvl == 1)
            toggleGameObjects.taskCompletionCanvasStage01.SetActive(true);

        if (currentLvl == 2)
            toggleGameObjects.taskCompletionCanvasStage02.SetActive(true);

        if (currentLvl == 3)
            toggleGameObjects.taskCompletionCanvasStage03.SetActive(true);

        currentStageOfProcess = allTaskNamesInOrder[playerStats.currentLvl][0]; // Since allTaskNamesInOrder is zero based while levels are not, this statement is put before increasing the current level.
        playerStats.currentLvl += 1;

        FindStep01();
    }

    public void ChurningMotionCompletedBehavior(int timesCompleted)
    {
        Debug.Log("Current Level: " + playerStats.currentLvl);
        Debug.Log("Num Times Churning Completed: " + numTimesChurningCompleted);

        allowSensorCollisionDetectorOperation = false;
        numTimesChurningCompleted = timesCompleted;

        if (playerStats.currentLvl == 1)
        {
            ResetValuesAfterTaskCompletion();

            currentStageOfProcess = "Completed";

            ShowTaskEndMenu(playerStats.currentLvl);
            return;
        }

        else if (playerStats.currentLvl == 2)
        {
            if (numTimesChurningCompleted == 1 || numTimesChurningCompleted == 2)
            {
                Debug.Log("Allowed cooking detector operation.");
                allowCookingDetectorOperation = true;

                currentStageOfProcess = "Cooking";
                return;
            }
        }
    }

    public void CookingCompletedBehavior(int timesCompleted)
    {
        allowCookingDetectorOperation = false;
        numTimesCookingCompleted = timesCompleted;

        if (playerStats.currentLvl == 2)
        {
            if (numTimesCookingCompleted == 1)
            {
                allowSensorCollisionDetectorOperation = true;

                currentStageOfProcess = "Churning";
                return;
            }

            else if (numTimesCookingCompleted == 2)
            {
                ResetValuesAfterTaskCompletion();

                currentStageOfProcess = "Completed";

                ShowTaskEndMenu(playerStats.currentLvl);
                return;
            }
        }
    }

    public void FindStep01()
    {
        Debug.Log("Current stage of process: " + currentStageOfProcess);
        Debug.Log("Current level: " + playerStats.currentLvl);

        if (currentStageOfProcess == "Churning")
        {
            allowSensorCollisionDetectorOperation = true;
            return;
        }

        if (currentStageOfProcess == "Cooking")
        {
            allowCookingDetectorOperation = true;
            return;
        }
    }

    public void ResetValuesAfterTaskCompletion()
    {
        numTimesChurningCompleted = 0;
        numTimesCookingCompleted = 0;
    }
}
