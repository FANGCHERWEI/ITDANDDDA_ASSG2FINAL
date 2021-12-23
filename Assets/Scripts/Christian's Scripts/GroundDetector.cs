using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public PlayerStatistics playerStats;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Bowl" && playerStats.maxPercentagePoints > 0)
        {
            playerStats.maxPercentagePoints -= 3;

            if (playerStats.maxPercentagePoints < 0)
                playerStats.maxPercentagePoints = 0;
        }
    }
}
