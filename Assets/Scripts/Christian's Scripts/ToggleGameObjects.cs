using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObjects : MonoBehaviour
{
    public GameObject fire;
    public AudioSource fireSound;
    public PlayerStatistics playerStats;
    public TaskManager taskManager;

    public float cookingDuration;
    public bool allowCooking;

    // Below I made multiple taskCompletionCanvas variables instead of a taskCompletionList to make sure I will assign the right canvas in the right order.
    // This is because assigning the taskCompletionCanvas GameObjects to a list may cause confusion of the order and naming.

    public GameObject taskCompletionCanvasStage01;
    public GameObject taskCompletionCanvasStage02;

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

    public void StartLvl01()
    {
        playerStats.currentLvl = 1;
    }

    public void DeactivateTaskCompletionCanvasStage01()
    {
        playerStats.currentLvl = 2;
        taskCompletionCanvasStage01.SetActive(false);
        taskManager.gameStart = true;
    }

    public void DeactivateTaskCompletionCanvasStage02()
    {
        taskCompletionCanvasStage02.SetActive(false);
    }
}
