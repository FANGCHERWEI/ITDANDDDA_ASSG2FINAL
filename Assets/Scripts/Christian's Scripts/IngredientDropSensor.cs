using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientDropSensor : MonoBehaviour
{
    public PlayerStatistics playerStats;

    public float ingredientBodyDropPenalty;
    public float ingredientParticleDropPenalty;
    public float collisionStayTime;
    public string collisionEnterObjTag;

    private bool objectCollisionStatus;

    private void Start()
    {
        if (ingredientBodyDropPenalty == 0)
            // Makes sure there is always a penalty even when ingredientBodyDropPenalty has not been modified in the Unity inspector.
            ingredientBodyDropPenalty = 1;

        if (ingredientParticleDropPenalty == 0)
            // Makes sure there is always a penalty even when ingredientParticleDropPenalty has not been modified in the Unity inspector.
            ingredientParticleDropPenalty = 0.1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionStayTime = 0;
        collisionEnterObjTag = collision.gameObject.tag;
        objectCollisionStatus = true;

        if (collisionEnterObjTag == "IngredientBody" || collisionEnterObjTag == "IngredientParticle")
            StartCoroutine(CollisionTimeElapsed(collisionEnterObjTag));
    }

    private void OnCollisionExit(Collision collision)
    {
        objectCollisionStatus = false;
    }

    private void CalculateIngredientDropPenalty(string collisionTag)
    {
        if (collisionTag == "IngredientBody")
            playerStats.maxPercentagePoints -= ingredientBodyDropPenalty;

        else if (collisionTag == "IngredientParticle")
            playerStats.maxPercentagePoints -= ingredientParticleDropPenalty;
    }

    IEnumerator CollisionTimeElapsed(string collisionTag)
    {
        yield return new WaitForSeconds(0.3f);

        if (objectCollisionStatus)
            CalculateIngredientDropPenalty(collisionTag);
    }
}
