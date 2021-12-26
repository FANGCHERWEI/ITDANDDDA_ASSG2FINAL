using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewPlayer
{
    // properties of new player
    public string userName;
    public string email;
    public bool active;
    public long lastLoggedIn;
    public long createdOn;
    public long updatedOn;

    // create new class
    public NewPlayer()
    {

    }

    // content of new class
    public NewPlayer(string userName, string email, bool active = true)
    {
        this.userName = userName;
        this.email = email;
        this.active = active;

        // timestamp properties
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        this.lastLoggedIn = timestamp;
        this.createdOn = timestamp;
        this.updatedOn = timestamp;
    }

    // convert to JSON
    public string NewPlayerToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
