using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For coder's reference:
// Layer 6 == "IngredientBody"
// Layer 7 == "IngredientParticle"
// Layer 8 == "LiquidContainer"
// Layer 9 == "IngredientsContainer"
// !!!!! == For UI developers

public class CookingDetector : MonoBehaviour
{
    public ContainerDetector containerDetector;
    public TaskManager taskManager;
    public ToggleGameObjects toggleGameObjects;
    public PlayerStatistics playerStats;

    private bool allIngredientsContained;

    public float cookingDurationSecs;
    public float timeRemaining;
    public int numTimesTaskCompleted;
    public int allowedLvl;

    public bool panCollidedWithStove;
    public bool allowCountdown;

    private void Awake()
    {
        allowedLvl = containerDetector.allowedLvl;
        allowCountdown = true;

        if (cookingDurationSecs == 0)
        {
            cookingDurationSecs = 5f;
        }

        timeRemaining = cookingDurationSecs;
        numTimesTaskCompleted = 0;
    }

    private void Update()
    {
        allIngredientsContained = containerDetector.allIngredientsContained;

        if (!toggleGameObjects.allowCooking)
        {
            allowCountdown = true;
        }

        if (allIngredientsContained && taskManager.allowCookingDetectorOperation && 
            allowedLvl == playerStats.currentLvl)
        {
            if (toggleGameObjects.allowCooking)
            {
                Debug.Log("CookingDetector.cs reached here at least.");
                // StartCoroutine(Countdown());
  
                timeRemaining -= Time.deltaTime; // !!!!! This statement stores the time remaining for cooking after the button is pressed. !!!!!
                Debug.Log("Time Remaining: " + timeRemaining);

                if (timeRemaining <= 0)
                {
                    numTimesTaskCompleted += 1;

                    toggleGameObjects.ToggleCooker();
                    taskManager.CookingCompletedBehavior(numTimesTaskCompleted);
                }
            }

            else
            {
                timeRemaining = cookingDurationSecs;
            }
        }
    }
}
