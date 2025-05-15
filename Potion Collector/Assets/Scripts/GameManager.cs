using System;
using System.Collections;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentScore { get; private set; }
    public DateTime SessionStartTime { get; private set; }
    public string SessionId { get; private set; }

    private DatabaseReference dbRef;
    
    private bool gameHasEnded = false;
    private FirebaseAuth _auth;
    private FirebaseUser _user;
    private DatabaseReference _dbReference;

    public bool IsGameOver => gameHasEnded;
    
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
    
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => FirebaseInit.IsInitialized);

        // Force Firebase Database load
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result != DependencyStatus.Available)
            {
                Debug.LogError("Firebase initialization failed: " + task.Result);
            }
            else
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;
        
                SignInAnonymously();
            }
        });
        
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        Debug.Log("✅ dbRef initialized successfully.");
        StartGame();
        StartCoroutine(StartGameTimer(60f));
    }
    
    private void SignInAnonymously()
    {
        _auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully && !task.IsFaulted && !task.IsCanceled)
            {
                AuthResult result = task.Result;
                _user = result.User;

                _dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Signed in anonymously as: " + _user.UserId);
                Debug.Log("Firebase initialized successfully");
            }
            else
            {
                Debug.LogError("Firebase anonymous sign-in failed: " + task.Exception);
            }
        });
    }


    IEnumerator StartGameTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        EndGame();
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
        if (gameHasEnded) return;
        
        gameHasEnded = true;
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
        Debug.Log("SaveSessionData() called");

        if (dbRef == null)
        {
            Debug.LogError(" dbRef is null. Firebase may not be initialized.");
            return;
        }

        if (string.IsNullOrEmpty(SessionId))
        {
            Debug.LogError(" SessionId is null or empty.");
            return;
        }

        var sessionData = new SessionData
        {
            sessionId = SessionId,
            score = CurrentScore,
            startTime = SessionStartTime.ToString("o"),
            endTime = endTime.ToString("o")
        };

        string json = JsonUtility.ToJson(sessionData);
        Debug.Log($"✅ Saving session: {json}");

        dbRef.Child("sessions").Child(SessionId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            bool success = !task.IsFaulted && !task.IsCanceled;
            EventManager.Trigger("FirebaseSyncCompletedEvent", "saveSession", success);
            Debug.Log(" Firebase session save completed, success: " + success);
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
