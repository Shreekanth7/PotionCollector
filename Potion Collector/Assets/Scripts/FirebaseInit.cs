using System;
using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseInit : MonoBehaviour
{
    public static bool IsInitialized { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Optional: if persistent between scenes
    }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("✅ Firebase is ready.");
                IsInitialized = true;

                // Optionally trigger an event
                EventManager.Trigger("FirebaseInitializedEvent");
            }
            else
            {
                Debug.LogError($"❌ Firebase dependencies not resolved: {dependencyStatus}");
                IsInitialized = false;
            }
        });
    }
}