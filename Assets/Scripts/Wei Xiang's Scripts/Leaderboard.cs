using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard
{
    // properties of leaderboard
    public float level1HighScore;
    public float level2HighScore;
    public float averageHighScore;
    public float totalTimePlayed;
    public string userProfileRating;
    public string userName;

    // create new class
    public Leaderboard()
    {

    }

    // content of new class
    public Leaderboard(float level1HighScore, float level2HighScore, float averageHighScore, float totalTimePlayed, string userProfileRating, string userName)
    {
        this.level1HighScore = level1HighScore;
        this.level2HighScore = level2HighScore;
        this.averageHighScore = averageHighScore;
        this.totalTimePlayed = totalTimePlayed;
        this.userProfileRating = userProfileRating;
        this.userName = userName;
    }

    // convert to JSON
    public string LeaderboardToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
