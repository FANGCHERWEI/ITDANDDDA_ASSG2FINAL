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
    [SerializeField]
    private PlayerStatistics playerStats;

    public float ingredientDropPenalty;
    public float collisionStayTime; // Time an object can have when dropped on the table or floor before player loses points.

    private bool objectCollisionStatus;

    // The below variables set default values for their respective variables.
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
            playerStats.maxPercentagePointsLvl01 -= ingredientDropPenalty; // Reduces the points for level 1.
        }

        else if (playerStats.currentLvl == 2)
        {
            playerStats.maxPercentagePointsLvl02 -= ingredientDropPenalty; // Reduces the points for level 2.
        }
    }

    IEnumerator CollisionTimeElapsed(int collisionLayer)
    {
        /* 
        Waits for a certain amount of time before acknowledging that the dropped object is dropped before doing calculations. 
        Necessary to prevent miscalculations of object collision. One example is an object colliding with a collider that is NEAR a table or floor but not AT those locations.
        I.e. There is a container with a collider (such as a pan or bowl) right on top of an IngredientDropSensor's object with a collider (such as a floor).

        When the above example occurs and an ingredient falls to the container, OnCollisionEnter detects the ingredient as colliding with with the container collider AND the floor collider.

        However, once the ingredient stopped moving, OnCollisionExit instantly detects the ingredient as leaving the collider and the collision system detects the ingredient 
        as in contact with only the container. This causes the user to lose points even if the ingredient ended up in the bowl. To overcome this, 
        I set a timer to detect how long an object is in contact with IngredientDropSensor's object's collider. If the object is in contact for more than a certain period of time, 
        but not too long that the user can cheat by having quick reflexes, then the object has actually dropped.
        */

        yield return new WaitForSeconds(collisionStayTime);

        if (objectCollisionStatus && (collisionLayer == 6 || collisionLayer == 7))
        {
            CalculateIngredientDropPenalty();
        }
    }

    private void InitializeVariables()
    {
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
