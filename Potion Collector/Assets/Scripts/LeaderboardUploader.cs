using System;
using Firebase.Database;
using UnityEngine;

public class LeaderboardUploader : MonoBehaviour
{
    public void UploadScore(string sessionId, int score)
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        ScoreEntry entry = new ScoreEntry(sessionId, score, timestamp);

        string key = FirebaseDatabase.DefaultInstance
            .GetReference("leaderboard")
            .Push()
            .Key;

        FirebaseDatabase.DefaultInstance
            .GetReference("leaderboard")
            .Child(key)
            .SetRawJsonValueAsync(JsonUtility.ToJson(entry))
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("Score uploaded!");
                else
                    Debug.LogError("Upload failed: " + task.Exception);
            });
    }
    
    private void OnEnable()
    {
        EventManager.Subscribe("GameEndedEvent", OnGameEnded);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("GameEndedEvent", OnGameEnded);
    }

    void OnGameEnded(object[] args)
    {
        int score = (int)args[0];
        string sessionId = (string)args[1];
        UploadScore(sessionId, score);
    }

}