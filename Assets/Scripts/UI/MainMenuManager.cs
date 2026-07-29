using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Panel pop-up Pengaturan / Settings")]
    public GameObject settingsPanel;

    [Header("Audio (Opsional)")]
    [Tooltip("Slider pengatur volume di panel Settings")]
    public Slider volumeSlider;

    void Start()
    {
        // Sembunyikan panel settings saat pertama kali terbuka
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        // Inisialisasi slider volume jika ada
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    /// <summary>
    /// Pindah ke scene permainan (Level1)
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// Membuka panel Settings
    /// </summary>
    public void OpenSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Menutup panel Settings
    /// </summary>
    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Mengatur Volume Utama Game (0.0 - 1.0)
    /// </summary>
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    /// <summary>
    /// Keluar dari Game
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit Game dipanggil (Aplikasi ditutup)");
        Application.Quit();
    }
}
