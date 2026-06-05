using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScoreEntry
{
    public string name;
    public int score;
}

[System.Serializable]
public class HighScoreData
{
    public List<HighScoreEntry> scoresList = new List<HighScoreEntry>();
}

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;
    private const string PrefKey = "HighScoresData";
    private HighScoreData highScores = new HighScoreData();

    [HideInInspector] public bool showHighScoresOnLoad = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<HighScoreEntry> GetTopScores()
    {
        return highScores.scoresList;
    }

    public int GetHighestScore()
    {
        if (highScores.scoresList.Count > 0)
            return highScores.scoresList[0].score;
        return 0;
    }

    public void AddNewScore(string playerName, int playerScore)
    {
        if (string.IsNullOrEmpty(playerName)) playerName = "Anonymous";

        HighScoreEntry newEntry = new HighScoreEntry { name = playerName, score = playerScore };
        highScores.scoresList.Add(newEntry);

        highScores.scoresList.Sort((x, y) => y.score.CompareTo(x.score));

        if (highScores.scoresList.Count > 10)
        {
            highScores.scoresList.RemoveRange(10, highScores.scoresList.Count - 10);
        }

        SaveScores();
    }

    private void SaveScores()
    {
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString(PrefKey, json);
        PlayerPrefs.Save();
    }

    private void LoadScores()
    {
        if (PlayerPrefs.HasKey(PrefKey))
        {
            string json = PlayerPrefs.GetString(PrefKey);
            highScores = JsonUtility.FromJson<HighScoreData>(json);
        }
    }

    [ContextMenu("Wipe All High Scores")]
    public void ClearSavedLeaderboardData()
    {
        PlayerPrefs.DeleteKey(PrefKey);
        PlayerPrefs.Save();
        highScores = new HighScoreData();
        Debug.Log("<color=red>Leaderboard database reset successfully!</color>");
    }
}