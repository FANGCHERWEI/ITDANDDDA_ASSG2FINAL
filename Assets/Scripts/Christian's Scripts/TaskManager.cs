using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script manages the connection between different scripts.

public class TaskManager : MonoBehaviour
{
    private List<string[]> allTaskNamesInOrder = new List<string[]>();

    // Below variables contain coder's custom components.
    public PlayerStatistics playerStats;
    public ToggleGameObjects toggleGameObjects;
    public LevelCompleted levelCompleted;

    // Below variables will be used to update SensorCollisionDetector.cs script variables.
    public bool allowSensorCollisionDetectorOperation; // Allows or prevents the churning activity from occurring, thus allowing more control over when the SensorCollisionDetector.cs script can run. The same logic applies to other allowTaskOperation variables.
    public int numTimesChurningCompleted;

    // Below variables will be used to update CookingDetector.cs script variables.
    public bool allowCookingDetectorOperation;
    public int numTimesCookingCompleted;

    public static bool gameStart;
    public string currentStageOfProcess;
    public bool thing = true;
    public bool thing2 = true;

    public static bool completedLevel01 = false;
    public static bool completedLevel02 = false;

    // The below variables contain the steps for each task in order and are used to identify what order script(s) will run in each stage and what script(s) is/are currently running.
    private string[] stage01TaskNamesInOrder = { "Churning" };
    private string[] stage02TaskNamesInOrder = { "Churning", "Cooking", "Churning", "Cooking" };

    private void Awake()
    {
        InitializeVariables();
    }

    private void Update()
    {
        // Debug.Log("Gamestart status: " + gameStart);
        if (gameStart && thing)
        {
            CountdownTimer.timerIsRunning = true;
            SaveLevelData.userIDRetrieved = false;
            SaveLevelData.getUsernameFlag = true;
            SaveLevelData.highScoresRetrived = false;

            thing = false;
        }

        else if (!gameStart && thing2)
        {
            // This else statement resets the value of the countdown timer so that it will restart when changing levels.
            CountdownTimer.timeRemaining = 1801;
            CountdownTimer.timerIsRunning = false;

            thing2 = false;
        }
    }

    private void ShowTaskEndMenu(int currentLvl)
    {
        // Shows the menu after the player completes a task

        gameStart = false; // Stops the main timer from running.

        if (currentLvl == 1)
        {
            // toggleGameObjects.taskCompletedCanvasStage01.SetActive(true);

            levelCompleted.LevelCompletedData();
        }

        if (currentLvl == 2)
        {
            // toggleGameObjects.taskCompletedCanvasStage02.SetActive(true);

            levelCompleted.LevelCompletedData();
        }

        currentStageOfProcess = allTaskNamesInOrder[PlayerStatistics.currentLvl][0]; // Since allTaskNamesInOrder is zero based while levels are not, this statement reduces the current level by one.
        
        PlayerStatistics.currentLvl += 1;

        FindStep01();
    }

    public void ChurningMotionCompletedBehavior(int timesCompleted)
    {
        // The behavior of the game when a churning (e.g. whisking, stirring, etc.) task is completed.

        allowSensorCollisionDetectorOperation = false; 
        numTimesChurningCompleted = timesCompleted;

        if (PlayerStatistics.currentLvl == 1)
        {
            
            ResetValuesAfterTaskCompletion();

            currentStageOfProcess = "Completed";

            ShowTaskEndMenu(PlayerStatistics.currentLvl);
            return;
        }

        else if (PlayerStatistics.currentLvl == 2)
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
        // This function contains the behavior for the game when a cooking task is completed.

        allowCookingDetectorOperation = false;
        numTimesCookingCompleted = timesCompleted;

        if (PlayerStatistics.currentLvl == 2)
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

                ShowTaskEndMenu(PlayerStatistics.currentLvl);
                return;
            }
        }
    }

    public void FindStep01()
    {
        // Debug.Log("Current stage of process: " + currentStageOfProcess);
        // Debug.Log("Current level: " + PlayerStatistics.currentLvl);

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

    private void InitializeVariables()
    {
        // Assigns values to variables so that they are not empty and have the necessary values for the script to run.

        gameStart = true; // Allows the main timer to run.
        thing = true;
        thing2 = true;

        currentStageOfProcess = stage01TaskNamesInOrder[0];

        allTaskNamesInOrder.Add(stage01TaskNamesInOrder);
        allTaskNamesInOrder.Add(stage02TaskNamesInOrder);

        numTimesChurningCompleted = 0;
        // PlayerStatistics.currentLvl = 1; // Delete this later pls.

        allowSensorCollisionDetectorOperation = false;
        allowCookingDetectorOperation = false;

        FindStep01();
    }
}
