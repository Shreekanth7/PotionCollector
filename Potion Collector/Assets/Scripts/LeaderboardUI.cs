using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    public TMP_Text leaderboardText;
    public LeaderboardManager leaderboardManager;

    void Start()
    {
        leaderboardManager.LoadTopScores(DisplayLeaderboard);
    }

    void DisplayLeaderboard(List<ScoreEntry> topScores)
    {
        leaderboardText.text = "Top Scores:\n";
        foreach (var entry in topScores)
        {
            leaderboardText.text += $"Score: {entry.score}\n";
        }
    }
}