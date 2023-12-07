using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField]int minutes;
    [SerializeField]int seconds;
    float fSeconds;

    public TMP_Text timeDisplay;
    bool startTimer;

    public TargetManager targetManager;

    private void Start()
    {
        instance = this;
        targetManager = GetComponent<TargetManager>();
    }

    private void Update()
    {
        if (startTimer)
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

        timeDisplay.text = minutes.ToString() + ":" + seconds.ToString();
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(!startTimer)
            {
                StartTimer();
            }
        }
    }

    public void stopTimer()
    {
        startTimer = false;
        ApplyPenalty();
    }
    public void StartTimer()
    {
        startTimer = true;
    }

    void ApplyPenalty()
    {
        print(targetManager.GetBlueTargets());
        print(targetManager.redTargets.Count);
    }
}
