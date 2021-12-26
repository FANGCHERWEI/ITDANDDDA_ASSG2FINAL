using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * Coder's Reference:
 * !!!!! means this is an important variable for UI
 * This script detects whether respective ingredients are collided with their containers
 * Containers can be any item that holds other items, such as bowls, plates, etc.
*/

public class ContainerDetector : MonoBehaviour
{
    public Dictionary<string, GameObject> ingredientsToDetect = new Dictionary<string, GameObject>();

    private List<GameObject> ingredients = new List<GameObject>();
    private List<string> ingredientNames = new List<string>(); // !!!!! List of all ingredient names. !!!!!
    private List<string> missingIngredientsList = new List<string>(); // !!!!! List of missing ingredients from the current step. !!!!!

    // Below variables contain coder's custom components.
    private IngredientInfo ingredientInfoComponent;
    public PlayerStatistics playerStats;
    public TaskManager taskManager;

    public bool allIngredientsContained;
    public bool ingredientsMissing;

    [SerializeField]
    private bool allowCustomDebugFunctions;

    public bool allowContainerMovement;

    private string collisionEnterObjName;
    private string collisionExitObjName;
    private string initIngredientObjName;
    private string currentStageOfProcess;

    private GameObject collisionEnterObj;
    private GameObject collisionExitObj;


    public int allowedLvl; // What level this script can run in. It is different depending on which game object this script is located in.

    // The below variables set default values for their respective variables
    private bool defaultAllIngredientsContained = false;
    private bool defaultIngredientsMissing = false;
    private string defaultCurrentStageOfProcess = "Start";

    private void Awake()
    {
        InitializeVariables();
    }

    private void FixedUpdate()
    {
        if (playerStats.currentLvl == allowedLvl)
        {
            gameObject.GetComponent<Collider>().enabled = true;
        }

        else
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }

        currentStageOfProcess = taskManager.currentStageOfProcess;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IngredientInfo>() != null)
        {
            collisionEnterObj = collision.gameObject;
            collisionEnterObjName = collisionEnterObj.GetComponent<IngredientInfo>().customName;

            CheckForIngredientCollision(collisionEnterObjName, collisionEnter: true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<IngredientInfo>() != null)
        {
            collisionExitObj = collision.gameObject;
            collisionExitObjName = collisionExitObj.GetComponent<IngredientInfo>().customName;

            CheckForIngredientCollision(collisionExitObjName, collisionEnter: false);
        }
    }

    private void CheckForIngredientCollision(string collisionObjName, bool collisionEnter)
    {
        // This function checks whether the collided object is an ingredient.

        if (ingredientsToDetect.ContainsKey(collisionObjName))
        {
            if (collisionEnter)
            {
                // Checks whether the collision is a collisionEnter to determine whether ingredient is in the container instead of exiting the container
                ingredientsToDetect[collisionObjName].GetComponent<IngredientInfo>().isInContainer = true;
            }

            else
            {
                // If collision is collisionExit, it means the ingredient left the container, therefore it is not in the container anymore
                ingredientsToDetect[collisionObjName].GetComponent<IngredientInfo>().isInContainer = false;
            }
        }

        CheckIfContainsAllIngredients();
    }

    private void CheckIfContainsAllIngredients()
    {
        // This function checks if all ingredients are in the container (E.g. bowl) to allow stirring/whisking to happen with the WhiskDetector.cs script.

        ingredientsMissing = false;
        missingIngredientsList.Clear();

        foreach (KeyValuePair<string, GameObject> ingredientContained in ingredientsToDetect)
        {
            ingredientInfoComponent = ingredientContained.Value.GetComponent<IngredientInfo>();

            if (!ingredientInfoComponent.isInContainer &&
                ingredientInfoComponent.customTags.Contains(currentStageOfProcess))
            {
                missingIngredientsList.Add(ingredientInfoComponent.customName); // Used to display what necessary ingredients are missing for the step.
                ingredientsMissing = true;
            }
        }

        if (ingredientsMissing)
        {
            allIngredientsContained = false;
        }

        else
        {
            allIngredientsContained = true;
        }

    }

    private void InitializeVariables()
    {
        // This function sets relevant values to variables that need them before the game begins.

        allIngredientsContained = defaultAllIngredientsContained;
        ingredientsMissing = defaultIngredientsMissing;
        currentStageOfProcess = defaultCurrentStageOfProcess;

        foreach (GameObject ingredient in ingredients)
        {
            // Gets all the names of the Ingredient GameObjects and sets all their status to false to indicate that they are not in the container.

            try
            {
                initIngredientObjName = ingredient.GetComponent<IngredientInfo>().customName;

                ingredientsToDetect[initIngredientObjName] = ingredient;
                ingredientsToDetect[initIngredientObjName].GetComponent<IngredientInfo>().isInContainer = false;
                ingredientNames.Add(initIngredientObjName);
            }

            catch (Exception exception)
            {
                Debug.LogException(exception, this);
                Debug.Log("Check if all ingredients in ContainerDetector.cs script have IngredientInfo components");
            }
        }
    }

    private void Debug_IngredientsToDetectValueCheck(string identifier)
    {
        // Checks the values of the ingredientsToDetect variable
        // Identifier string helps to identify which function call is running.

        if (allowCustomDebugFunctions)
        {
            foreach (KeyValuePair<string, GameObject> anIngredient in ingredientsToDetect)
            {
                Debug.Log(anIngredient.Key + " is " + anIngredient.Value.GetComponent<IngredientInfo>().isInContainer + ". Identifier: " + identifier);
            }
        }
    }

    private void Debug_CheckForFunctionOperation(string currentFunctionName)
    {
        // Checks what function a piece of code is currently in so that other debug functions have context in the console.
        // Use System.Reflection.MethodBase.GetCurrentMethod().Name as the argument in this function call.

        Debug.Log("Currently in function " + currentFunctionName);

    }

    private void Debug_CheckForMissingIngredients(string identifier)
    {
        // Checks whether any ingredients are missing and what exactly are the missing ingredients.

        if (missingIngredientsList.Count == 0)
        {
            Debug.Log("No items are missing." + " identifier: " + identifier);
            return;
        }

        foreach (string missingIngredient in missingIngredientsList)
        {
            Debug.Log("Missing Ingredient: " + missingIngredient + " identifier: " + identifier);
        }
    }

    private void Debug_CheckIfLvlAllowed()
    {
        if (playerStats.currentLvl == allowedLvl)
        {
            Debug.Log("Current Lvl: " + playerStats.currentLvl + " Mesh: " + gameObject.name + " - Allowed");
        }

        else
        {
            Debug.Log("Current Lvl: " + playerStats.currentLvl + " Mesh: " + gameObject.name + " - Denied");
        }
    }
}
