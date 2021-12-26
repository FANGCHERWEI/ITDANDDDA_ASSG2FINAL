using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Deal with super high speeds.
// TODO: Turn on gravity on the whisker after testing

public class SensorCollisionDetector : MonoBehaviour
{
    // "sensors" used to be called "whiskerSensors" as this code was only used for whisking originally.
    // However, they are now used for other churning actions such as stirring. The renamed variables are used to generalize the code more.

    [SerializeField] private string collidedObjEnterName;
    [SerializeField] private int sensorsWithRequiredNumCollisions; // Keeps track of the sensors that detect circular movement such as whisking and stirring.
                                                                   // Prevents player from cheating by making sure they collide with all the sensors, which ensures they move in a circular motion.

    public Dictionary<string, int> timesEachSensorActivated = new Dictionary<string, int>(); // Contains how many times a sensor has been collided with.
    public List<GameObject> sensors = new List<GameObject>();

    public int numTimesTaskCompleted;
    public int requiredNumChurns; // The number of times a player has to complete a churning motion. Used for movements such as whisking and stirring.
    public bool churningMotionTaskCompleted;
    public bool showChurningTool;
    public float delayBeforeLvlComplete;
    public bool allowRunCoroutine;
    
    private bool allIngredientsContained;
    private int allowedLvl;
    
    public ContainerDetector containerDetector;
    public PlayerStatistics playerStats;
    public TaskManager taskManager;
    public GameObject churningTool; // E.g. Whisks/equipment used for churning.

    private void Start()
    {
        InitializeVariables();
    }

    private void FixedUpdate()
    {
        allIngredientsContained = containerDetector.allIngredientsContained;

        if (taskManager.allowSensorCollisionDetectorOperation && allIngredientsContained && allowedLvl == playerStats.currentLvl)
        {
            // Debug.Log("taskManager.allowSensorCollisionDetectorOperation: " + taskManager.allowSensorCollisionDetectorOperation +
            //           ", allIngredientsContained: " + allIngredientsContained);

            GetComponent<Collider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
        }

        else if ((!taskManager.allowSensorCollisionDetectorOperation || !allIngredientsContained) && allowedLvl == playerStats.currentLvl)
        {
            // Debug.Log("taskManager.allowSensorCollisionDetectorOperation: " + taskManager.allowSensorCollisionDetectorOperation +
            //           ", allIngredientsContained: " + allIngredientsContained);

            GetComponent<Collider>().enabled = false;
        }

        else
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Collision with: " + collision.gameObject.name);
        if (!timesEachSensorActivated.ContainsKey(collision.gameObject.name))
        {
            // This else if statement ignores triggerEnter with ingredients and other things inside the container. The only exception being sensors such as WhiskDetectors.
            // It prevents weird collisions between the churning tool and ingredients.
            // Debug.Log("Collision should be ignored.");
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Debug.Log("Trigger: " + collision.gameObject.name);

        collidedObjEnterName = collision.gameObject.name;

        if (timesEachSensorActivated.ContainsKey(collidedObjEnterName) && allIngredientsContained)
        {
            // Debug.Log("Churning tool collided with sensor. Sensor: " + collidedObjEnterName);
            CheckSensorCollision(collidedObjEnterName);
        }

        else if (!timesEachSensorActivated.ContainsKey(collidedObjEnterName))
        {
            // This else if statement ignores triggerEnter with ingredients and other things inside the container. The only exception being sensors such as WhiskDetectors.
            // It prevents weird collisions between the churning tool and ingredients.
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }

        if (churningMotionTaskCompleted)
        {
            Debug.Log("Churning motion task completed.");

            gameObject.GetComponent<MeshRenderer>().enabled = false;

            numTimesTaskCompleted += 1;
            taskManager.numTimesChurningCompleted = numTimesTaskCompleted;
            sensorsWithRequiredNumCollisions = 0;

            taskManager.ChurningMotionCompletedBehavior(numTimesTaskCompleted);

            foreach (GameObject sensor in sensors)
            {
                timesEachSensorActivated[sensor.name] = 0;
            }

            churningMotionTaskCompleted = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), false);
    }

    void CheckSensorCollision(string whiskSensorName)
    {
        // Checks whether all sensors have the required number of activations to allow for completion of the task.
        // I.e. Checks whether the user has completed the number of circular motions necessary to complete the task.

        sensorsWithRequiredNumCollisions = 0;

        timesEachSensorActivated[whiskSensorName] += 1;

        foreach (KeyValuePair<string, int> whiskSensor in timesEachSensorActivated)
        {
            if (whiskSensor.Value >= requiredNumChurns)
            {
                sensorsWithRequiredNumCollisions += 1;
            }

            if (sensorsWithRequiredNumCollisions == sensors.Count)
            {
                Debug.Log("All sensors have the required number of collisions");
                churningMotionTaskCompleted = true;
            }
        }
    }

    void InitializeVariables()
    {
        // This function sets relevant values to variables that need them before the game begins

        allowedLvl = containerDetector.allowedLvl;

        if (requiredNumChurns == 0)
        {
            requiredNumChurns = 3;
        }

        allIngredientsContained = false;
        churningMotionTaskCompleted = false;
        numTimesTaskCompleted = 0;

        foreach (GameObject sensor in sensors)
        {
            timesEachSensorActivated.Add(sensor.name, 0);
        }
    }
}
