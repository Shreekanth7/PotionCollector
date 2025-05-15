using UnityEngine;
using TMPro; // Required for TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text scoreText;

    private void OnEnable()
    {
        EventManager.Subscribe("ScoreUpdatedEvent", OnScoreUpdated);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("ScoreUpdatedEvent", OnScoreUpdated);
    }

    void Start()
    {
        UpdateScoreText(0); // Initialize to 0
    }

    private void OnScoreUpdated(object[] args)
    {
        int newScore = (int)args[0];
        UpdateScoreText(newScore);
    }

    private void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
        else
        {
            Debug.LogWarning("Score Text is not assigned in UIManager.");
        }
    }
}