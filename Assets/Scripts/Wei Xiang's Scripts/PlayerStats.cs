using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    // properties of player stats
    public string username;
    public float score;
    public string rating;
    public float timePlayed;

    // new class
    public PlayerStats()
    {

    }

    // content of new class
    public PlayerStats(string username, float score, string rating, float timePlayed)
    {
        this.username = username;
        this.score = score;
        this.rating = rating;
        this.timePlayed = timePlayed;
    }

    // convert to JSON
    public string UsernameToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
