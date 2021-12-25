using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining;
    public static float timeElapsed;
    public static bool timerIsRunning = false;
    public GameObject timeUI;
    public string timeDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // set the timer to start running
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if timer is running, and time remaining is more than 0, let timer run
        if (timerIsRunning)
        {
            SaveLevelData.convertTimeFlag = true;
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timeElapsed += Time.deltaTime;
            }

            // if timer is running but time remaining is less than or equals to 0, set timer running to false and reset time remaining to 30 minutes
            else
            {
                timeRemaining = 1800;
                timerIsRunning = false;
                // do stuff when player fails
            }
        }

        // update the ui to display float in minutes and seconds
        DisplayTime(timeRemaining);
    }

    // update the ui to display float in minutes and seconds
    public void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // format the time in minutes and seconds
        timeDisplay = string.Format("{0:00}:{1:00}", minutes, seconds);
        // update the ui to display float in minutes and seconds
        timeUI.GetComponent<Text>().text = timeDisplay;
    }
}
