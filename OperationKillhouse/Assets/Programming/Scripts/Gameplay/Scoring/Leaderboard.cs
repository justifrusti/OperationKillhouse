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
    public TMP_InputField time;
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
        string names = "";
        string time = "";

        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            names += leaderboardEntries[i].name + ",";
            time += leaderboardEntries[i].score + ",";
        }

        saveData.names = names;
        saveData.time = time;

        UpdateLeaderboardVisual();
    }

    void UpdateLeaderboardVisual()
    {
        display.text = "";
        time.text = "";

        for (int i = 0; i <= leaderboardEntries.Count - 1; i++)
        {
            display.text += leaderboardEntries[i].name + "\n";
            time.text += leaderboardEntries[i].score + "\n";
        }

        SaveManager.Save(saveData);
    }

    void LoadLeaderBoard()
    {
        string name = saveData.names;
        string time = saveData.time;

        string[] name2 = name.Split(',');
        string[] time2 = time.Split(",");

        for (int i = 0; i < name2.Length - 1; i++)
        {
            PlayerInfo loadedInfo = new PlayerInfo(name2[i], time2[i], scoreInSeconds);

            leaderboardEntries.Add(loadedInfo);

            UpdateLeaderboardVisual();
        }
    }

    public void ClearData()
    {
        SaveManager.DeleteData();

        display.text = "";
        time.text = "";
    }
}
