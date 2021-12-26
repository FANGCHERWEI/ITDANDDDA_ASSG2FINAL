using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script contains the functions that buttons will use.

public class ToggleGameObjects : MonoBehaviour
{
    public GameObject fire;
    public GameObject taskCompletedCanvasStage01;
    public GameObject taskCompletedCanvasStage02;

    public AudioSource fireSound;

    // Below variables contain coder's custom components.
    public PlayerStatistics playerStats;
    public TaskManager taskManager;

    public float cookingDuration;
    public bool allowCooking; 

    private void Awake()
    {
        allowCooking = false;

        if (cookingDuration == 0)
        {
            cookingDuration = 5f;
        }
    }

    public void ToggleCooker()
    {
        if (!allowCooking)
        {
            fire.SetActive(true);
            fireSound.Play();

            allowCooking = true;
        }

        else
        {
            fire.SetActive(false);

            fireSound.Stop();

            allowCooking = false;
        }
    }

    public void StartCanvasButton()
    {
        playerStats.currentLvl = 1;
    }

    public void DeactivateTaskCompletionCanvasStage01()
    {
        // Deactivates the stage 1 task completion canvas.

        playerStats.currentLvl = 2; // Changes the current level to 2.
        taskCompletedCanvasStage01.SetActive(false);
        taskManager.gameStart = true; // Allows the timer to start running again.
    }

    public void DeactivateTaskCompletionCanvasStage02()
    {
        // Deactivates the stage 2 task completion canvas.

        taskCompletedCanvasStage02.SetActive(false);
    }
}
