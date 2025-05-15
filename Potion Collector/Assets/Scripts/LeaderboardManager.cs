using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public void LoadTopScores(System.Action<List<ScoreEntry>> onScoresLoaded)
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("leaderboard")
            .OrderByChild("score")
            .LimitToLast(5)
            .GetValueAsync()
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to load leaderboard.");
                    return;
                }

                List<ScoreEntry> topScores = new List<ScoreEntry>();
                foreach (var child in task.Result.Children)
                {
                    string json = child.GetRawJsonValue();
                    ScoreEntry entry = JsonUtility.FromJson<ScoreEntry>(json);
                    topScores.Add(entry);
                }

                // Firebase returns ascending order, so we reverse
                topScores = topScores.OrderByDescending(e => e.score).ToList();
                onScoresLoaded?.Invoke(topScores);
            });
    }
}