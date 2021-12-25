using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowLevels
{
    // properties of allowing levels
    public bool level1Played;
    public bool level2Played;

    public bool allowLevel2;

    // create new class
    public AllowLevels()
    {

    }

    // content of new class
    public AllowLevels(bool level1Played, bool level2Played, bool allowLevel2)
    {
        this.level1Played = level1Played;
        this.level2Played = level2Played;
        this.allowLevel2 = allowLevel2;
    }

    // convert to JSON
    public string AllowLevelsToJson()
    {
        return JsonUtility.ToJson(this);
    }

}
