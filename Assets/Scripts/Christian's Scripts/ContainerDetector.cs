using System.Collections.Generic;
using UnityEngine;

// This script detects whether respective ingredients are collided with their containers
// Containers can be any item that holds other items, such as bowls, plates, etc.

public class ContainerDetector : MonoBehaviour
{
    public Dictionary<string, bool> ingredientsToDetect = new Dictionary<string, bool>();
    public List<GameObject> ingredients = new List<GameObject>();

    public bool containsAllIngredients;

    private string collisionEnterObjName;
    private string collisionExitObjName;

    private void Awake()
    {
        InitializeVariables();
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionEnterObjName = collision.gameObject.name;

        Debug.Log(gameObject.name + " ENTERED collision with: " + collisionEnterObjName);

        CheckForIngredientCollision(collisionEnterObjName, collisionEnter: true);
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionExitObjName = collision.gameObject.name;

        Debug.Log(gameObject.name + " EXITED collision with: " + collisionExitObjName);

        CheckForIngredientCollision(collisionExitObjName, collisionEnter: false);
    }

    private void CheckForIngredientCollision(string collisionObjName, bool collisionEnter)
    {
        // This function checks whether an ingredient collided with the container instead of colliding with another object that has a collider

        Debug.Log("Entered CheckForIngredientCollision() function in BowlDetector.cs script.");

        if (ingredientsToDetect.ContainsKey(collisionObjName))
        {
            if (collisionEnter)
                // Checks whether the collision is a collisionEnter to determine whether ingredient is in the container instead of exiting the container
                ingredientsToDetect[collisionObjName] = true;

            else
                // If collision is collisionExit, it means the ingredient left the container, therefore it is not in the container anymore
                ingredientsToDetect[collisionObjName] = false;
        }

        containsAllIngredients = CheckIfContainsAllIngredients();

        Debug.Log("Exited CheckForIngredientCollision() function in BowlDetector.cs script.");
    }

    private void InitializeVariables()
    {
        // This function sets relevant values to variables that need them before the game begins

        Debug.Log("Entered InitializeVariables() function in BowlDetector.cs script.");

        containsAllIngredients = false;

        foreach (GameObject ingredient in ingredients) 
        {
            // Gets all the names of the Ingredient GameObjects and sets all their status to false to indicate that they are not in the container.
            ingredientsToDetect[ingredient.name] = false;

            Debug.Log(ingredient.name + " status is: " + ingredientsToDetect[ingredient.name]);
        }

        Debug.Log("Exited InitializeVariables() function in BowlDetector.cs script.");

    }

    private bool CheckIfContainsAllIngredients()
    {
        // This function checks if all ingredients are in the container (E.g. bowl) to allow stirring/whisking to happen with the WhiskDetector.cs script.

        Debug.Log("Entered CheckIfContainsAllIngredients() function in BowlDetector.cs script.");

        foreach (KeyValuePair<string, bool> ingredientContained in ingredientsToDetect)
        {
            if (ingredientContained.Value == false)
            {
                Debug.Log("Exited CheckIfContainsAllIngredients() function in BowlDetector.cs script. Returned false.");

                return false;
            }
        }

        Debug.Log("Exited CheckIfContainsAllIngredients() function in BowlDetector.cs script. Returned true.");
        return true;
    }
}
