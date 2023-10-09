using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField]int minutes;
    [SerializeField]int seconds;
    float fSeconds;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        fSeconds += Time.deltaTime * 1;

        if(fSeconds >= 1)
        {
            fSeconds -= 1;
            seconds++;
        }

        if (seconds >= 60)
        {
            seconds -= 60;
            minutes++;
        }
    }

    public string GetTime()
    {
        string totalTime = minutes.ToString() + ":" + seconds.ToString();

        return totalTime;
    }

    public int GetTimeInSeconds()
    {
        return seconds + (minutes * 60);
    }

    public void AddPenalty(int penaltyScore)
    {
        seconds = (seconds + penaltyScore);
    }
}
