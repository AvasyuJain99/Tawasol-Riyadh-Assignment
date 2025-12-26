using UnityEngine;
using UnityEngine.SceneManagement;

namespace SyncDash.Managers
{
    /// <summary>
    /// Main game manager - handles game state, scoring, and game flow
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("Game Settings")]
        [SerializeField] private bool gameStarted = false;
        [SerializeField] private bool gameOver = false;
        
        [Header("Score Settings")]
        [SerializeField] private float distanceScoreMultiplier = 1f;
        [SerializeField] private int collectibleScoreValue = 10;
        
        private float totalDistance = 0f;
        private int collectibleScore = 0;
        private int totalScore = 0;
        
        // Events
        public System.Action<int> OnScoreChanged;
        public System.Action OnGameStarted;
        public System.Action OnGameOver;
        
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            StartGame();
        }
        
        public void StartGame()
        {
            gameStarted = true;
            gameOver = false;
            totalDistance = 0f;
            collectibleScore = 0;
            totalScore = 0;
            
            OnGameStarted?.Invoke();
            UpdateScore();
        }
        
        public void EndGame()
        {
            if (gameOver) return;
            
            gameOver = true;
            gameStarted = false;
            
            // Save high score
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            if (totalScore > highScore)
            {
                PlayerPrefs.SetInt("HighScore", totalScore);
                PlayerPrefs.Save();
            }
            
            OnGameOver?.Invoke();
        }
        
        public void AddDistance(float distance)
        {
            if (!gameStarted || gameOver) return;
            
            totalDistance += distance;
            UpdateScore();
        }
        
        public void CollectOrb()
        {
            if (!gameStarted || gameOver) return;
            
            collectibleScore += collectibleScoreValue;
            UpdateScore();
        }
        
        private void UpdateScore()
        {
            totalScore = Mathf.RoundToInt(totalDistance * distanceScoreMultiplier) + collectibleScore;
            OnScoreChanged?.Invoke(totalScore);
        }
        
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        // Getters
        public bool IsGameStarted() => gameStarted;
        public bool IsGameOver() => gameOver;
        public int GetScore() => totalScore;
        public int GetHighScore() => PlayerPrefs.GetInt("HighScore", 0);
        public float GetDistance() => totalDistance;
    }
}

