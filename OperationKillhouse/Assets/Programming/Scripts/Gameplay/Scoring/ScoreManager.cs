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
    [Space]
    public int wrongTargetPenalty;
    [Space]
    public TMP_Text timeDisplay;
    public TMP_Text blueTagetPenalty;
    public TMP_Text redTargetsLeft;

    bool startTimer;
    [Space]
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
    void ApplyPenalty()
    {
        blueTagetPenalty.gameObject.SetActive(true);
        redTargetsLeft.gameObject.SetActive(true);

        AddPenalty(targetManager.GetBlueTargets() * wrongTargetPenalty);
        blueTagetPenalty.text = ("You shot " + targetManager.GetBlueTargets() + " and gained a " + targetManager.GetBlueTargets() * 10 + " time penalty. your final time is " + GetTime());


        redTargetsLeft.text = ("There where " + targetManager.redTargets.Count + " red targets left");
        
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
}
