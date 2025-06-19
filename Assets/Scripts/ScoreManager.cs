using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    [Header("Score Settings")]
    public float pointsPerSecond = 5f;
    public float survivalTime = 0f;
    public int currentScore = 0;
    
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    
    private bool isGameActive = true;
    private BlacksmithHealth blacksmithHealth;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Find the blacksmith health component
        blacksmithHealth = FindObjectOfType<BlacksmithHealth>();
        if (blacksmithHealth == null)
        {
            Debug.LogError("BlacksmithHealth not found in scene!");
        }

        // Initialize UI
        UpdateScoreUI();
        UpdateTimeUI();
    }

    void Update()
    {
        if (!isGameActive || blacksmithHealth == null) return;

        // Only accumulate score if the blacksmith is alive
        if (blacksmithHealth.IsAlive())
        {
            // Add points based on time
            survivalTime += Time.deltaTime;
            int newPoints = Mathf.FloorToInt(pointsPerSecond * Time.deltaTime);
            AddPoints(newPoints);
            
            // Update UI
            UpdateTimeUI();
            UpdateScoreUI();
        }
        else
        {
            GameOver();
        }
    }

    public void AddPoints(int points)
    {
        currentScore += points;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }

    void UpdateTimeUI()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(survivalTime / 60);
            int seconds = Mathf.FloorToInt(survivalTime % 60);
            timeText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    void GameOver()
    {
        if (!isGameActive) return;
        
        isGameActive = false;
        Debug.Log($"Game Over! Final Score: {currentScore}, Survival Time: {survivalTime:F2} seconds");
        
        // You could trigger game over UI or other game over logic here
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public float GetSurvivalTime()
    {
        return survivalTime;
    }
} 