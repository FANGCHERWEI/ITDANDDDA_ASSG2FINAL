using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompleted : MonoBehaviour
{
    public GameObject dataObject;

    // LEVEL COMPLETED
    public string rating1;
    public float currentLevel1Score;
    public string timePlayedLevel1Output;

    public string rating2;
    public float currentLevel2Score;
    public string timePlayedLevel2Output;

    public GameObject levelCompleteRating;
    public GameObject levelCompleteScore;
    public GameObject duration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelCompletedData()
    {
        rating1 = dataObject.GetComponent<SaveLevelData>().ratingLevel1;
        currentLevel1Score = dataObject.GetComponent<SaveLevelData>().currentLevel1Score;
        timePlayedLevel1Output = dataObject.GetComponent<SaveLevelData>().timePlayedLevel1Output;

        rating2 = dataObject.GetComponent<SaveLevelData>().ratingLevel2;
        currentLevel2Score = dataObject.GetComponent<SaveLevelData>().currentLevel2Score;
        timePlayedLevel2Output = dataObject.GetComponent<SaveLevelData>().timePlayedLevel2Output;

        if (PlayerStatistics.currentLvl == 1)
        {
            levelCompleteRating.GetComponent<TMP_Text>().text = string.Format("Your Ratings : {0}", rating1); ;
            levelCompleteScore.GetComponent<TMP_Text>().text = string.Format("Your Score : \n{0}", currentLevel1Score);
            duration.GetComponent<TMP_Text>().text = string.Format("Duration : {0}", timePlayedLevel1Output);
        }

        if (PlayerStatistics.currentLvl == 2)
        {
            levelCompleteRating.GetComponent<TMP_Text>().text = string.Format("Your Ratings : {0}", rating2); ;
            levelCompleteScore.GetComponent<TMP_Text>().text = string.Format("Your Score : \n{0}", currentLevel2Score);
            duration.GetComponent<TMP_Text>().text = string.Format("Duration : {0}", timePlayedLevel2Output);
        }
    }
}
