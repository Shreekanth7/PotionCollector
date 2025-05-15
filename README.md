# Potion Collector - Unity Mini-Game

Potion Collector is a lightweight Android mini-game where players gather magical potions across a whimsical environment. Designed with performance and scalability in mind, the game utilizes Unity Addressables, a custom event-driven architecture, and Firebase for backend support.

---

## 🕹️ Game Overview and Interaction Flow

**Gameplay Objective:**
- Collect as many potions as possible within a time limit.
- Avoid fake potions that deduct points.

**Core Interaction Flow:**
1. **Main Menu** – Start Game, View Leaderboard.
2. **Gameplay Scene** – Move player and collect potions.
3. **Game Over** – Show score and optionally submit to Firebase leaderboard.
4. **Leaderboard** – View top scores from Firebase.

Players control a character using on-screen joystick buttons. Potions appear randomly, and each collected potion triggers feedback via the event system.

---

## 🔥 Firebase Setup Guide (Exclude API Keys)

Firebase is used for:
- Leaderboard data (saving and retrieving top scores)
- Basic analytics (event tracking like "PotionCollected")

**Steps to Set Up Firebase:**
1. Go to [Firebase Console](https://console.firebase.google.com/) and create a new project.
2. Enable **Authentication** (optional) and **Realtime Database** or **Firestore**.
3. Download the `google-services.json` file and place it in your Unity project's `Assets` folder.
4. Install the following Firebase SDKs via Unity Package Manager or Unity Firebase SDK:
   - Firebase Core
   - Firebase Database (or Firestore)
   - Firebase Analytics (optional)
5. Initialize Firebase in a bootstrap scene or singleton.

```csharp
FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
    var dependencyStatus = task.Result;
    if (dependencyStatus == DependencyStatus.Available) {
        FirebaseApp app = FirebaseApp.DefaultInstance;
    }
});
