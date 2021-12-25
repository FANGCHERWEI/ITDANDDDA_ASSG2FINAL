using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
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

    // PROFILE PANEL
    public string username;
    public string userProfileRating;
    public string totalTimePlayed;
    public float averageHighScore;
    public int updatedOn;
    public float overall;
    public float level1HighScore;
    public float level2HighScore;

    public GameObject usernameHeader;
    public GameObject usernameSmallHeader;
    public GameObject userProfileRatingObject;
    public GameObject totalGameTime;
    public GameObject yourAverageScore;
    public GameObject dateUpdated;
    public GameObject overallObject;
    public GameObject level1HighScoreObject;
    public GameObject level2HighScoreObject;

    // LEADERBOARD
    public List<string> leaderboardByUserID;
    public List<float> leaderboardByAverageHighScore;
    public List<string> leaderboardByUsername;
    public List<string> leaderboardByUserProfileRating;
    public List<float> leaderboardByTotalTimePlayed;

    public GameObject rank1Name;
    public GameObject rank2Name;
    public GameObject rank3Name;

    public GameObject rank1Score;
    public GameObject rank2Score;
    public GameObject rank3Score;

    public GameObject rank1Rating;
    public GameObject rank2Rating;
    public GameObject rank3Rating;

    public GameObject rank1Time;
    public GameObject rank2Time;
    public GameObject rank3Time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // LEVEL COMPLETED
    public void LevelCompletedData()
    {
        // check for the level that is being played now
        //levelCompleteRating.GetComponent<TMP_Text>().text = string.Format("Your Ratings: {}", );
    }

    // PROFILE PANEL
    public void Profile()
    {
        username = dataObject.GetComponent<SaveLevelData>().username;
        userProfileRating = dataObject.GetComponent<SaveLevelData>().userProfileRating;
        totalTimePlayed = dataObject.GetComponent<SaveLevelData>().totalTimePlayedOutput;
        averageHighScore = dataObject.GetComponent<SaveLevelData>().averageHighScore;
        updatedOn = dataObject.GetComponent<SaveLevelData>().updatedOn;
        level1HighScore = dataObject.GetComponent<SaveLevelData>().level1HighScore;
        level2HighScore = dataObject.GetComponent<SaveLevelData>().level2HighScore;
        overall = level1HighScore + level2HighScore;

        usernameHeader.GetComponent<TMP_Text>().text = string.Format("{0}'s Profile", username);
        usernameSmallHeader.GetComponent<TMP_Text>().text = username;
        userProfileRatingObject.GetComponent<TMP_Text>().text = string.Format("Your Rating: {0}", userProfileRating);
        totalGameTime.GetComponent<TMP_Text>().text = string.Format("Total Game Time: {0}", totalTimePlayed);
        yourAverageScore.GetComponent<TMP_Text>().text = string.Format("Your Average Score: {0}", averageHighScore);
        dateUpdated.GetComponent<TMP_Text>().text = string.Format("Date Updated: {0}", updatedOn);
        overallObject.GetComponent<TMP_Text>().text = string.Format("Overall: {0}", overall);
        level1HighScoreObject.GetComponent<TMP_Text>().text = string.Format("Level 1: {0}", level1HighScore);
        level2HighScoreObject.GetComponent<TMP_Text>().text = string.Format("Level 2: {0}", level2HighScore);

    }

    // LEADERBOARD
    public void Leaderboard()
    {
        leaderboardByUserID = dataObject.GetComponent<SaveLevelData>().leaderboardByUserID;
        leaderboardByAverageHighScore = dataObject.GetComponent<SaveLevelData>().leaderboardByAverageHighScore;
        leaderboardByUsername = dataObject.GetComponent<SaveLevelData>().leaderboardByUsername;
        leaderboardByUserProfileRating = dataObject.GetComponent<SaveLevelData>().leaderboardByUserProfileRating;
        leaderboardByTotalTimePlayed = dataObject.GetComponent<SaveLevelData>().leaderboardByTotalTimePlayed;


        rank1Name.GetComponent<TMP_Text>().text = leaderboardByUsername[0];
        rank2Name.GetComponent<TMP_Text>().text = leaderboardByUsername[1];
        rank3Name.GetComponent<TMP_Text>().text = leaderboardByUsername[2];

        rank1Score.GetComponent<TMP_Text>().text = leaderboardByAverageHighScore[0].ToString();
        rank2Score.GetComponent<TMP_Text>().text = leaderboardByAverageHighScore[1].ToString();
        rank3Score.GetComponent<TMP_Text>().text = leaderboardByAverageHighScore[2].ToString();

        rank1Rating.GetComponent<TMP_Text>().text = leaderboardByUserProfileRating[0];
        rank2Rating.GetComponent<TMP_Text>().text = leaderboardByUserProfileRating[1];
        rank3Rating.GetComponent<TMP_Text>().text = leaderboardByUserProfileRating[2];

        rank1Time.GetComponent<TMP_Text>().text = DisplayTime(leaderboardByTotalTimePlayed[0]);
        rank2Time.GetComponent<TMP_Text>().text = DisplayTime(leaderboardByTotalTimePlayed[1]);
        rank3Time.GetComponent<TMP_Text>().text = DisplayTime(leaderboardByTotalTimePlayed[2]);

    }

    public string DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // format the time in minutes and seconds
        string timeDisplay = string.Format("{0:00}:{1:00}", minutes, seconds);
        return timeDisplay;
    }
}
