using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public float totalTime = 60f;

    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    
    private float currentTime;
    private bool isRunning = false;

    private void OnEnable()
    {
        EventManager.Subscribe("ScoreUpdatedEvent", OnScoreUpdated);
        EventManager.Subscribe("GameEndedEvent", OnGameEnded);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("ScoreUpdatedEvent", OnScoreUpdated);
        EventManager.Unsubscribe("GameEndedEvent", OnGameEnded);
    }

    void OnGameEnded(object[] args)
    {
        int finalScore = (int)args[1];
        finalScoreText.text = $"Final Score: {finalScore}";
        gameOverPanel.SetActive(true);
    }
    
    public void StartTimer()
    {
        currentTime = totalTime;
        isRunning = true;
        UpdateTimerUI();
    }
    
    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }
    
    
    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        UpdateTimerUI();

        if (currentTime <= 0f)
        {
            isRunning = false;
            currentTime = 0f;
        }
    }
    
    void Start()
    {
        StartTimer();
        UpdateScoreText(0);
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