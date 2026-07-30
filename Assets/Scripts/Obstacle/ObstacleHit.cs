using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ObstacleHit : MonoBehaviour
{
    private AudioSource audioSource;
    private bool crashed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (crashed) return;

        // Deteksi apakah objek yang menabrak atau parent-nya memiliki skrip PlayerController (roda/bodi mobil)
        PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            crashed = true;

            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }

            StartCoroutine(RestartScene());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (crashed) return;

        // Deteksi jika berupa trigger (misal mobil menyentuh rintangan trigger)
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            crashed = true;

            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }

            StartCoroutine(RestartScene());
        }
    }

    IEnumerator RestartScene()
    {
        // Menunggu delay singkat (0.8 detik) setelah menabrak agar terasa dramatis
        yield return new WaitForSeconds(0.8f);

        if (GameUIManager.instance != null)
        {
            GameUIManager.instance.ShowGameOver();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}