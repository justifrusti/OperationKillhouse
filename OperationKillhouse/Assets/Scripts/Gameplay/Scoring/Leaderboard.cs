using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfo
{
    public string name;
    public string score;

    public int iScore;

    public PlayerInfo(string name, string score, int iScore)
    {
        this.name = name;
        this.score = score;
        this.iScore = iScore;
    }
}

public class Leaderboard : MonoBehaviour
{
    public TMP_InputField userName;
    public TMP_InputField display;

    public string score;

    public SaveData saveData;

    List<PlayerInfo> leaderboardEntries;
    private int scoreInSeconds;

    private void Start()
    {
        saveData = SaveManager.Load();
        leaderboardEntries = new List<PlayerInfo>();
        LoadLeaderBoard();
    }

    private void Update()
    {
        score = ScoreManager.instance.GetTime();
        scoreInSeconds = ScoreManager.instance.GetTimeInSeconds();
    }

    public void SubmitScore()
    {
        PlayerInfo stats = new PlayerInfo(userName.text, score, scoreInSeconds);

        leaderboardEntries.Add(stats);
        userName.text = "PlayerName";

        SortStats();
    }

    void SortStats()
    {
        for (int i = leaderboardEntries.Count - 1; i > 0; i--)
        {
            if (leaderboardEntries[i].iScore < leaderboardEntries[i - 1].iScore)
            {
                PlayerInfo tempInfo = leaderboardEntries[i];

                leaderboardEntries[i] = leaderboardEntries[i - 1];
                leaderboardEntries[i - 1] = tempInfo;
            }
        }

        UpdatePlayerPrefabString();
    }

    void UpdatePlayerPrefabString()
    {
        string stats = "";

        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            stats += leaderboardEntries[i].name + ",";
            stats += leaderboardEntries[i].score + ",";
        }

        saveData.leaderboardEntries = stats;

        UpdateLeaderboardVisual();
    }

    void UpdateLeaderboardVisual()
    {
        display.text = "";

        for (int i = 0; i <= leaderboardEntries.Count - 1; i++)
        {
            display.text += leaderboardEntries[i].name + ": " + leaderboardEntries[i].score + "\n";
        }

        SaveManager.Save(saveData);
    }

    void LoadLeaderBoard()
    {
        string stats = saveData.leaderboardEntries;

        string[] stats2 = stats.Split(',');

        for (int i = 0; i < stats2.Length - 2; i += 2)
        {
            PlayerInfo loadedInfo = new PlayerInfo(stats2[i], score, scoreInSeconds);

            leaderboardEntries.Add(loadedInfo);

            UpdateLeaderboardVisual();
        }
    }

    public void ClearData()
    {
        SaveManager.DeleteData();

        display.text = "";
    }
}
