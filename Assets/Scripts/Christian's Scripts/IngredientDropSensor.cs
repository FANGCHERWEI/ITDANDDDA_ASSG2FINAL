using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
Coder's Reference:
CollisionLayer 6 == IngredientBody
CollisionLayer 7 == IngredientParticle
*/

public class IngredientDropSensor : MonoBehaviour
{
    // Below variable contains coder's custom components.
    [SerializeField]
    private PlayerStatistics playerStats;

    public float ingredientDropPenalty;
    public float collisionStayTime; // Time an object can have when dropped on the table or floor before player loses points.

    private bool objectCollisionStatus;

    // The below variables set the default values for their respective variables.
    private float defaultIngredientDropPenalty = 4;
    private float defaultCollisionStayTime = 0.3f;
    private bool defaultObjectCollisionStatus = false;

    private void Awake()
    {
        InitializeVariables();
    }

    private void OnCollisionEnter(Collision collision)
    {
        int collisionObjLayer = collision.gameObject.layer;
        
        objectCollisionStatus = true;

        if (collisionObjLayer == 6 || collisionObjLayer == 7)
        {
            StartCoroutine(CollisionTimeElapsed(collisionObjLayer));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        objectCollisionStatus = false;
    }

    private void CalculateIngredientDropPenalty()
    {
        // Calculates how many points are lost and relates it to the level that the player is currently in.

        // The bottom if-else statement makes sure that the player loses points only if INGREDIENTS are dropped, not any other object.
        if (playerStats.currentLvl == 1)
        {
            playerStats.PercentagePointsLvl01 -= ingredientDropPenalty; // Reduces the points for level 1.
        }

        else if (playerStats.currentLvl == 2)
        {
            playerStats.PercentagePointsLvl02 -= ingredientDropPenalty; // Reduces the points for level 2.
        }
    }

    IEnumerator CollisionTimeElapsed(int collisionLayer)
    {
        // Waits for a certain amount of time before acknowledging that the dropped object is  on the ground/table dropped before doing calculations

        yield return new WaitForSeconds(collisionStayTime);

        if (objectCollisionStatus && (collisionLayer == 6 || collisionLayer == 7))
        {
            CalculateIngredientDropPenalty();
        }
    }

    private void InitializeVariables()
    {
        // Assigns values to variables so that they are not empty and have the necessary values for the script to run.

        if (ingredientDropPenalty == 0)
        {
            // This if statement makes sure there is always a penalty even when ingredientBodyDropPenalty is not modified in the inspector.
            ingredientDropPenalty = defaultIngredientDropPenalty;
        }

        if (collisionStayTime == 0)
        {
            // This if statement makes sure there is always a length of time to check how long an object is on an IngredientDropSensor object for.
            collisionStayTime = defaultCollisionStayTime;
        }

        objectCollisionStatus = defaultObjectCollisionStatus;
    }
}
