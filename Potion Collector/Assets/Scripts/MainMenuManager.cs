using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject leaderboardPanel;

    [Header("Leaderboard Content")]
    public Transform leaderboardContent;
    public GameObject scoreEntryPrefab;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        leaderboardPanel.SetActive(false);
    }

    public void OnStartGameClicked()
    {
        // Load your actual gameplay scene here
        SceneManager.LoadScene("GamePlay"); // Replace with your scene name
    }

    public void OnLeaderboardClicked()
    {
        mainMenuPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }

    public void OnBackClicked()
    {
        leaderboardPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    
}
