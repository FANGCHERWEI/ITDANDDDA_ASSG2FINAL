using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class SaveLevelData : MonoBehaviour
{
    // initialise firebase auth
    Firebase.Auth.FirebaseAuth auth;
    public DatabaseReference dbReference;

    public string userID;
    public string username;

    public static bool userIDRetrieved;
    public static bool getUsernameFlag = false;
    public static bool highScoresRetrived = false;

    public float level1HighScore;
    public float level2HighScore;
    public float averageHighScore;

    public float currentLevel1Score;
    public float currentLevel2Score;

    public string ratingLevel1;
    public string ratingLevel2;
    public string userProfileRating;

    public float timePlayedLevel1;
    public float timePlayedLevel2;

    public string timePlayedLevel1Output;
    public string timePlayedLevel2Output;

    public static bool convertTimeFlag;

    public float totalTimePlayed;
    public string totalTimePlayedOutput;

    public bool level1Played;
    public bool level2Played;
    public bool allowLevel2;

    public int updatedOn;

    public List<string> leaderboardByUserID = new List<string>();
    public List<float> leaderboardByAverageHighScore = new List<float>();
    public List<string> leaderboardByUsername = new List<string>();
    public List<string> leaderboardByUserProfileRating = new List<string>();
    public List<float> leaderboardByTotalTimePlayed = new List<float>();


    // initialise auth instance
    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Start is called before the first frame update
    void Start()
    {
        // userID has not been retrieved
        userIDRetrieved = false;
        // get username is allowed
        getUsernameFlag = true;
        // high scores have no been retrieved
        highScoresRetrived = false;
        // convert time is allowed
        convertTimeFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        // get the boolean from leaderboard ui thing (not finalised)

        // if user is logged in and userID has not been retrieved, get the userID from another script and set userID retrieved to be true
        if (Authentication.loggedIn && !userIDRetrieved)
        {
            userID = Authentication.userID;
            // get the time played data
            GetTimePlayed();
            // get the timestamp for updatedOn
            GetDateUpdated();
            // convert total time played into minutes and seconds
            DisplayTotalGameTime(totalTimePlayed);
            // get the leaderboard data
            GetLeaderboard();
            //GetLeaderboard();
            // get user profile rating
            GetUserProfileRating();
            userIDRetrieved = true;
        }

        // if userID has been retrieved and get username is allowed, get the username through the userID on the database, and do not allow to get username
        if (userIDRetrieved && getUsernameFlag)
        {
            if (Authentication.firstTimeLogin)
            {
                username = Authentication.usernameSignUp;
            }

            else
            {
                GetUsername();
                getUsernameFlag = false;
            }
        }

        // after username has been retrieved, we would have to retrieve the high scores of the player
        if (!getUsernameFlag && !highScoresRetrived)
        {
            GetLevel1HighScore();
            GetLevel2HighScore();
            GetAverageHighScore();
            // reset current scores
            currentLevel1Score = 0;
            currentLevel2Score = 0;
            // reset timePlayed
            timePlayedLevel1 = 0;
            timePlayedLevel2 = 0;
            // reset current ratings
            ratingLevel1 = "";
            ratingLevel2 = "";
            // get allow levels from the database
            GetAllowLevels();
            highScoresRetrived = true;
        }

        if (!CountdownTimer.timerIsRunning && CountdownTimer.timeElapsed != 0 && convertTimeFlag)
        {
            // check for the current level that is played
            // retrieve the value of the score
            //if (currentlevelis1)
            {
                // retrieve the score for level 1
                // currentLevel1Score = 
                // save the time taken to complete level 1 into timePlayedLevel1
                timePlayedLevel1 = CountdownTimer.timeElapsed;
                // convert the time that is in float to minutes and seconds
                DisplayTimePlayedLevel1(timePlayedLevel1);
            }

            //else if (currentlevelis2)
            {
                // retrieve the score for level 2
                // currentLevel2Score = 
                // save the time taken to complete level 2
                timePlayedLevel2 = CountdownTimer.timeElapsed;
                // convert the time taken to complete level 2 into minutes and seconds
                DisplayTimePlayedLevel2(timePlayedLevel2);
            }

            convertTimeFlag = false;
        }
    }

    // save game data of level 1 to the database
    public void SaveDataLevel1()
    {
        // checks if the score is within each rating and gives the respective rating
        if (currentLevel1Score >= 0 && currentLevel1Score < 50)
        {
            ratingLevel1 = "Fail";
        }

        else if (currentLevel1Score >= 50 && currentLevel1Score < 80)
        {
            ratingLevel1 = "Good";
        }

        else if (currentLevel1Score >= 80 && currentLevel1Score <= 100)
        {
            ratingLevel1 = "Excellent";
        }

        // save game data
        PlayerStatsLevel1(userID, username, currentLevel1Score, ratingLevel1, timePlayedLevel1);
        // updates database that level 1 is played and level 2 is allowed
        Level1Played();
        // retrieve allow level to update the availability of other levels
        GetAllowLevels();

        // add the time played to the total time played
        totalTimePlayed += timePlayedLevel1;
        // update the total time played in the database
        dbReference.Child("leaderboards/" + userID + "/totalTimePlayed").SetValueAsync(totalTimePlayed);
        // reset the time played in level 1
        CountdownTimer.timeElapsed = 0;
        // allow convert time again
        convertTimeFlag = true;

        // if the current level 1 score is higher than the highest score the player has ever gotten in level 1, update database to replace the high score
        if (currentLevel1Score > level1HighScore)
        {
            // update the new level 1 high score
            level1HighScore = currentLevel1Score;
            dbReference.Child("leaderboards/" + userID + "/level1HighScore").SetValueAsync(currentLevel1Score);
            // update average score
            averageHighScore = (level1HighScore + level2HighScore) / 2;
            dbReference.Child("leaderboards/" + userID + "/averageHighScore").SetValueAsync(averageHighScore);

            // updates the user profile rating based on the new average high score
            if (averageHighScore >= 0 && averageHighScore < 50)
            {
                userProfileRating = "Fail";
            }

            else if (averageHighScore >= 50 && averageHighScore < 80)
            {
                userProfileRating = "Good";
            }

            else if (averageHighScore >= 80 && averageHighScore <= 100)
            {
                userProfileRating = "Excellent";
            }

            // uploads the new user profile rating to the database
            dbReference.Child("leaderboards/" + userID + "/userProfileRating").SetValueAsync(userProfileRating);
        }

        // update the timestamp of player updated
        UpdateTime();
        // retrieve leaderboard again
        GetLeaderboard();
        // get updated timestamp
        GetDateUpdated();
    }

    // save game data of level 2 to the database
    public void SaveDataLevel2()
    {
        // checks if the score is within each rating and gives the respective rating
        if (currentLevel2Score >= 0 && currentLevel2Score < 50)
        {
            ratingLevel2 = "Fail";
        }

        else if (currentLevel2Score >= 50 && currentLevel2Score < 80)
        {
            ratingLevel2 = "Good";
        }

        else if (currentLevel2Score >= 80 && currentLevel2Score <= 100)
        {
            ratingLevel2 = "Excellent";
        }

        // upload game data to database
        PlayerStatsLevel2(userID, username, currentLevel2Score, ratingLevel2, timePlayedLevel2);
        // updates database that level 2 is played and level 3 is allowed
        Level2Played();
        // retrieve allow level to update the availability of other levels
        GetAllowLevels();
        // allow convert time again
        convertTimeFlag = true;

        // updates the total time played
        totalTimePlayed += timePlayedLevel2;
        // upload the new total time played to the database
        dbReference.Child("leaderboards/" + userID + "/totalTimePlayed").SetValueAsync(totalTimePlayed);
        // reset the time taken to complete level 2
        CountdownTimer.timeElapsed = 0;

        // if the current level 2 score is higher than the highest score the player has ever gotten in level 2, update database to replace the high score
        if (currentLevel2Score > level2HighScore)
        {
            // update new level 2 high score
            level2HighScore = currentLevel2Score;
            dbReference.Child("leaderboards/" + userID + "/level2HighScore").SetValueAsync(currentLevel2Score);
            // update average score
            averageHighScore = (level1HighScore + level2HighScore) / 2;
            dbReference.Child("leaderboards/" + userID + "/averageHighScore").SetValueAsync(averageHighScore);

            // updates the user profile rating based on the new average high score
            if (averageHighScore >= 0 && averageHighScore < 50)
            {
                userProfileRating = "Fail";
            }

            else if (averageHighScore >= 50 && averageHighScore < 80)
            {
                userProfileRating = "Good";
            }

            else if (averageHighScore >= 80 && averageHighScore <= 100)
            {
                userProfileRating = "Excellent";
            }

            // uploads the new user profile rating to the database
            dbReference.Child("leaderboards/" + userID + "/userProfileRating").SetValueAsync(userProfileRating);
        }

        // update the timestamp of player updated
        UpdateTime();
        // retrieve leaderboard again
        GetLeaderboard();
        // get updated timestamp
        GetDateUpdated();
    }

    // reference to player stats class
    public void PlayerStatsLevel1(string uuid, string username, float score, string rating, float timePlayed)
    {
        PlayerStats playerStats = new PlayerStats(username, score, rating, timePlayed);
        // generate a random push key
        string key = dbReference.Child(uuid).Push().Key;

        // set the path to where the data would be stored
        dbReference.Child("playerStats/level1/" + uuid + "/" + key).SetRawJsonValueAsync(playerStats.UsernameToJson());
    }

    // reference to player stats class
    public void PlayerStatsLevel2(string uuid, string username, float score, string rating, float timePlayed)
    {
        PlayerStats playerStats = new PlayerStats(username, score, rating, timePlayed);
        // generate a random push key
        string key = dbReference.Child(uuid).Push().Key;

        // set the path to where the data would be stored
        dbReference.Child("playerStats/level2/" + uuid + "/" + key).SetRawJsonValueAsync(playerStats.UsernameToJson());
    }

    // function to change the bool in the database that level 1 has been played and allow level 2 to be played
    public void Level1Played()
    {
        dbReference.Child("allowLevels/" + userID + "/level1Played").SetValueAsync(true);
        dbReference.Child("allowLevels/" + userID + "/allowLevel2").SetValueAsync(true);
    }

    // function to change the bool in the database that level 2 has been played and allow level 3 to be played
    public void Level2Played()
    {
        dbReference.Child("allowLevels/" + userID + "/level2Played").SetValueAsync(true);
    }


    // get level 1 high score from the database
    public void GetLevel1HighScore()
    {
        dbReference.Child("leaderboards/" + userID + "/level1HighScore").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // retrieve level1HighScore and set it to the variable level1HighScore
                string result = snapshot.Value.ToString();
                level1HighScore = float.Parse(result);
            }
        });
    }

    // get level 2 high score from the database
    public void GetLevel2HighScore()
    {
        dbReference.Child("leaderboards/" + userID + "/level2HighScore").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // retrieve level2HighScore and set it to the variable level2HighScore
                string result = snapshot.Value.ToString();
                level2HighScore = float.Parse(result);
            }
        });
    }

    // get average high score from the database
    public void GetAverageHighScore()
    {
        dbReference.Child("leaderboards/" + userID + "/averageHighScore").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // retrieve averageHighScore and set it to the variable averageHighScore
                string result = snapshot.Value.ToString();
                averageHighScore = float.Parse(result);
            }
        });
    }

    // get allow levels from the database
    public void GetAllowLevels()
    {
        dbReference.Child("allowLevels/" + userID + "/level1Played").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // retrieve level1Played and set it to the variable level1Played
                string result = snapshot.Value.ToString();
                level1Played = Convert.ToBoolean(result);
            }
        });

        dbReference.Child("allowLevels/" + userID + "/level2Played").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // retrieve level2Played and set it to the variable level2Played
                string result = snapshot.Value.ToString();
                level2Played = Convert.ToBoolean(result);
            }
        });

        dbReference.Child("allowLevels/" + userID + "/allowLevel2").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // retrieve allowLevel2 and set it to the variable allowLevel2
                string result = snapshot.Value.ToString();
                allowLevel2 = Convert.ToBoolean(result);
            }
        });
    }

    // get the leaderboard from the database
    public void GetLeaderboard()
    {
        leaderboardByUserID.Clear();
        leaderboardByAverageHighScore.Clear();
        leaderboardByUsername.Clear();

        dbReference.Child("leaderboards").OrderByChild("averageHighScore").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    foreach (var child in snapshot.Children)
                    {
                        string id = child.Key;
                        leaderboardByUserID.Add(id);

                        string highScore = child.Child("averageHighScore").Value.ToString();
                        leaderboardByAverageHighScore.Add(float.Parse(highScore));

                        leaderboardByUsername.Add(child.Child("userName").Value.ToString());

                        leaderboardByUserProfileRating.Add(child.Child("userProfileRating").Value.ToString());

                        string playtime = child.Child("totalTimePlayed").Value.ToString();
                        leaderboardByTotalTimePlayed.Add(float.Parse(playtime));
                    }

                    leaderboardByUserID.Reverse();
                    leaderboardByAverageHighScore.Reverse();
                    leaderboardByUsername.Reverse();
                    leaderboardByUserProfileRating.Reverse();
                    leaderboardByTotalTimePlayed.Reverse();

                    int listCount = leaderboardByUserID.Count;

                    if (leaderboardByUserID.Count > 3)

                        for (int i = 0; i < listCount - 3; i++)
                        {
                            leaderboardByUserID.RemoveAt(3);
                            leaderboardByAverageHighScore.RemoveAt(3);
                            leaderboardByUsername.RemoveAt(3);
                            leaderboardByUserProfileRating.RemoveAt(3);
                            leaderboardByTotalTimePlayed.RemoveAt(3);
                        }
                }

                else
                {
                    Debug.Log("snapshot not found");
                }
            }
        });
    }

    // get the total time played by the player from the database
    public void GetTimePlayed()
    {
        dbReference.Child("leaderboards/" + userID + "/totalTimePlayed").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string result = snapshot.Value.ToString();
                totalTimePlayed = float.Parse(result);
            }
        });
    }

    // get the user profile rating from the database
    public void GetUserProfileRating()
    {
        dbReference.Child("leaderboards/" + userID + "/userProfileRating").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string result = snapshot.Value.ToString();
                userProfileRating = result;
            }
        });
    }

    // function to get username through userID from the database
    public void GetUsername()
    {
        dbReference.Child("players/" + userID + "/userName").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // retrieve username and set it to the variable username
                username = snapshot.Value.ToString();
            }
        });
    }

    // get date updated from the database
    public void GetDateUpdated()
    {
        dbReference.Child("players/" + userID + "/updatedOn").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Something went wrong when reading the data, ERROR: " + task.Exception);
                return;
            }

            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // retrieve username and set it to the variable username
                updatedOn = int.Parse(snapshot.Value.ToString());
            }
        });
    }

    // function to update the timestamp when updating data in the database
    public void UpdateTime()
    {
        // timestamp properties
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        // updates the last logged in timestamp for the player
        dbReference.Child("players/" + userID + "/updatedOn").SetValueAsync(timestamp);
    }

    // function to convert float into minutes and seconds for level 1
    public void DisplayTimePlayedLevel1(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timePlayedLevel1Output = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // function to convert float into minutes and seconds for level 2
    public void DisplayTimePlayedLevel2(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timePlayedLevel2Output = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // function to convert float into minutes and seconds for total time played
    public void DisplayTotalGameTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // format the time in minutes and seconds
        totalTimePlayedOutput = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
