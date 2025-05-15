using System;
using System.Collections;
using UnityEngine;
using Firebase.Database;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentScore { get; private set; }
    public DateTime SessionStartTime { get; private set; }
    public string SessionId { get; private set; }

    private DatabaseReference dbRef;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(WaitForFirebase());
    }

    private IEnumerator WaitForFirebase()
    {
        while (!FirebaseInit.IsInitialized)
            yield return null;

        Debug.Log("Firebase is now safe to use.");
        StartGame();
    }
    
    public void StartGame()
    {
        CurrentScore = 0;
        SessionStartTime = DateTime.Now;
        SessionId = Guid.NewGuid().ToString();

        Debug.Log("Game Started");
        EventManager.Trigger("GameStartedEvent", SessionStartTime, SessionId);
    }


    public void PauseGame()
    {
        Time.timeScale = 0f;

        Debug.Log("Game Paused");
        EventManager.Trigger("GamePausedEvent", DateTime.Now);
    }


    public void ResumeGame()
    {
        Time.timeScale = 1f;

        Debug.Log("Game Resumed");
        EventManager.Trigger("GameResumedEvent", DateTime.Now);
    }


    public void EndGame()
    {
        DateTime endTime = DateTime.Now;
        string sessionId = Guid.NewGuid().ToString();
        EventManager.Trigger("GameEndedEvent", sessionId, CurrentScore);
        SaveSessionData(endTime);
    }

    public void AddScore(int value)
    {
        int previousScore = CurrentScore;
        CurrentScore += value;
        EventManager.Trigger("ScoreUpdatedEvent", CurrentScore, value);
    }

    private void SaveSessionData(DateTime endTime)
    {
        EventManager.Trigger("FirebaseSyncStartedEvent", "saveSession");

        var sessionData = new SessionData
        {
            sessionId = SessionId,
            score = CurrentScore,
            startTime = SessionStartTime.ToString("o"),
            endTime = endTime.ToString("o")
        };

        string json = JsonUtility.ToJson(sessionData);

        dbRef.Child("sessions").Child(SessionId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            bool success = !task.IsFaulted && !task.IsCanceled;
            EventManager.Trigger("FirebaseSyncCompletedEvent", "saveSession", success);
        });
    }

    [Serializable]
    private class SessionData
    {
        public string sessionId;
        public int score;
        public string startTime;
        public string endTime;
    }
}
