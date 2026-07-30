using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject pausePanel;

    [Header("UI Text")]
    public TMP_Text victoryScoreText;

    private bool isPaused = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Pastikan panel disembunyikan di awal permainan
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        
        // Pastikan waktu berjalan normal
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Update()
    {
        // Memicu Menu Pause dengan tombol Escape atau P
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            // Jangan pause jika sedang Game Over atau Victory
            if ((gameOverPanel == null || !gameOverPanel.activeSelf) && 
                (victoryPanel == null || !victoryPanel.activeSelf))
            {
                TogglePause();
            }
        }

        // Shortcut Pengujian Developer (Hanya aktif di Unity Editor)
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("[Dev Shortcut] Memicu Game Over");
            ShowGameOver();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("[Dev Shortcut] Memicu Victory");
            ShowVictory(120); // Tes dengan skor 120
        }
        #endif
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        Time.timeScale = 0f; // Hentikan waktu permainan
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1f; // Kembalikan waktu normal
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        // Jeda waktu permainan
        Time.timeScale = 0f;
    }

    public void ShowVictory(int finalScore)
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        
        if (victoryScoreText != null)
        {
            victoryScoreText.text = "Final Score: " + finalScore;
        }
        
        // Jeda waktu permainan
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        // Kembalikan timeScale ke 1 sebelum memuat ulang scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Cek apakah scene berikutnya valid dalam build settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Jika tidak ada level selanjutnya, kembali ke main menu
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
