using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Deal with super high speeds.
// TODO: 4 separate colliders for the whisker
// TODO: Turn on gravity on the whisker after testing
// TODO: Turn off mesh renderer on whisk detector after testing
// TODO: Fix weird collision interaction with bowl

public class SensorCollisionDetector : MonoBehaviour
{
    // "sensors" used to be called "whiskerSensors" as this code was only used for whisking originally.
    // However, they are now used for other circular motion actions such as stirring. The renaming is used to generalize the code more.

    private string collidedObjEnterName;
    private string collidedObjExitName;
    private int sensorsWithRequiredNumCollisions; // Keeps track of the sensors that detect circular movement such as whisking and stirring.
                                                  // Prevents player from cheating by making sure they collide with all the sensors, which ensures they move in a circular motion.

    public Dictionary<string, int> timesEachSensorActivated = new Dictionary<string, int>(); // Contains how many times a sensor has been collided with.
    public List<GameObject> sensors = new List<GameObject>();

    public int requiredNumFullCircularMvmnts; // The number of times a player has to complete a circular motion. Used for movements such as whisking and stirring.
    //public bool allIngredientsContained;
    private bool allIngredientsContained;
    public bool whiskTaskComplete;
    
    public ContainerDetector containerDetector;
    public GameObject taskCompletionDisplay;
    public GameObject rotationTool; // E.g. Whisks, equipment used for stirring.

    private void Awake()
    {
        InitializeVariables();
    }

    private void Update()
    {
        allIngredientsContained = containerDetector.containsAllIngredients;
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(gameObject.name + " ENTERED trigger with: " + collision.gameObject.name);

        sensorsWithRequiredNumCollisions = 0;

        collidedObjEnterName = collision.gameObject.name;

        // TODO: Check velocity

        if (timesEachSensorActivated.ContainsKey(collidedObjEnterName) && allIngredientsContained)
        {
            CheckSensorCollision(collidedObjEnterName);
        }

        else if (!timesEachSensorActivated.ContainsKey(collidedObjEnterName))
        {
            // Ignores triggerEnter with ingredients and other things inside the container. The only exception being sensors such as WhiskDetectors;
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }
        
        if (whiskTaskComplete)
            ShowTaskEndMenu();
    }

    private void OnTriggerExit(Collider collision)
    {
        Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), false);
    }

    void InitializeVariables()
    {
        // This function sets relevant values to variables that need them before the game begins

        Debug.Log("Entered InitializeVariables() function in WhiskDetector.cs script.");

        if (requiredNumFullCircularMvmnts == 0)
            requiredNumFullCircularMvmnts = 3;

        allIngredientsContained = false;
        whiskTaskComplete = false;

        foreach (GameObject sensor in sensors)
        {
            timesEachSensorActivated.Add(sensor.name, 0);
        }

        Debug.Log("Exited InitializeVariables() function in WhiskDetector.cs script.");
    }

    void CheckSensorCollision(string whiskSensorName)
    {
        // Checks whether all sensors have the required number of activations to allow for completion of the task.
        // I.e. Checks whether the user has completed the number of circular motions necessary to complete the task.

        Debug.Log("Entered CheckSensorCollision() function in WhiskDetector.cs script.");

        timesEachSensorActivated[whiskSensorName] += 1;

        Debug.Log("WhiskSensor " + whiskSensorName + " num of activations: " + timesEachSensorActivated[whiskSensorName]);

        foreach (KeyValuePair<string, int> whiskSensor in timesEachSensorActivated)
        {
            if (whiskSensor.Value >= requiredNumFullCircularMvmnts)
                sensorsWithRequiredNumCollisions += 1;

            if (sensorsWithRequiredNumCollisions == sensors.Count)
                whiskTaskComplete = true;
        }

        Debug.Log("Exited CheckSensorCollision() function in WhiskDetector.cs script.");
    }

    void ShowTaskEndMenu()
    {
        // Shows the menu after the player completes a task

        Debug.Log("Entered ShowTaskEndMenu() function in WhiskDetector.cs script.");

        Debug.Log("Whisking motion complete.");

        rotationTool.SetActive(false);
        taskCompletionDisplay.SetActive(true);

        Debug.Log("Exited ShowTaskEndMenu() function in WhiskDetector.cs script.");
    }

}
