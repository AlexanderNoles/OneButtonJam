using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerControl : MonoBehaviour
{
    public static float currentTime;
    private TextMeshProUGUI text;
    private bool startTimeSet = false;
    private float startTime;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (!PlayerManagment.gameOver) 
        {
            if (!startTimeSet && Time.timeSinceLevelLoad > 0)
            {
                startTime = Time.realtimeSinceStartup;
                startTimeSet = true;
            }
            currentTime = Time.realtimeSinceStartup - startTime;
            string timeAsString = currentTime.ToString();
            if (timeAsString.Contains("E"))
            {
                text.text = string.Empty;
            }
            else
            {
                text.text = currentTime.ToString();
            }
        }
        else
        {
            text.text = "";
        }
    }
}
