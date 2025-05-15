using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class FirebaseInit : MonoBehaviour
{
    public static bool IsInitialized { get; private set; }
    public static FirebaseUser CurrentUser { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject); 
    }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log(" Firebase is ready.");
                IsInitialized = true;

                // Proceed with anonymous login
                SignInAnonymously();

                // Optionally trigger your event
                EventManager.Trigger("FirebaseInitializedEvent");
            }
            else
            {
                Debug.LogError($" Firebase dependencies not resolved: {dependencyStatus}");
                IsInitialized = false;
            }
        });
    }

    private void SignInAnonymously()
    {
        FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync().ContinueWithOnMainThread((Task<AuthResult> task) =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError(" Anonymous sign-in was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError(" Anonymous sign-in failed: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result.User;
            CurrentUser = newUser;
            Debug.Log(" Signed in anonymously. UID: " + newUser.UserId);
        });
    }


}