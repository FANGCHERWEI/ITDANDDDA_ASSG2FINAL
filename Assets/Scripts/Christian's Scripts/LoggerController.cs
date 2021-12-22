using UnityEngine;

public class LoggerController : MonoBehaviour
{
    // Controls whether logging for all my scripts are allowed. Logging is turned off to save processing power.

    public bool allowDisplayLogs;

    private void Awake()
    {
        if (!allowDisplayLogs)
            Debug.unityLogger.logEnabled = false;
    }
}
