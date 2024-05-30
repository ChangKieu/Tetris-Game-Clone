using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject gameOverPanel;
    private AudioSource audioSource;
    [SerializeField] private AudioClip moveSound, dropSound, winSound, gameOverSound, clearSound;

    private int score = 0;
    private int highScore = 0;
    private bool gameOver = false;
    private const string HighScoreKey = "HighScore";


    private void Start()
    {
        LoadHighScore();
        UpdateScoreUI();
        audioSource= GetComponent<AudioSource>();
    }
    private void Update()
    {
        
    }
    public void SetGameOver(bool result)
    {
        gameOver = result;
        if (gameOver)
        {
            audioSource.PlayOneShot(gameOverSound);
            gameOverPanel.SetActive(true);
        }
    }
    public bool IsGameOver()
    {
        return gameOver;
    }
    public void AddScore(int linesCleared)
    {
        score += linesCleared;
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore();
        }
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
        highScoreText.text = PlayerPrefs.GetInt(HighScoreKey).ToString();
    }

    private void LoadHighScore()
    {
        if (PlayerPrefs.HasKey(HighScoreKey))
        {
            highScore = PlayerPrefs.GetInt(HighScoreKey);
        }
        else
        {
            highScore = 0;
            SaveHighScore();
        }
    }
    public void PlayMoveSound()
    {
        audioSource.PlayOneShot(moveSound);
    }
    public void PlayDropSound()
    {
        audioSource.PlayOneShot(dropSound);
    }
    public void PlayClearSound()
    {
        audioSource.PlayOneShot(clearSound);
    }
    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.Save();
    }
}
