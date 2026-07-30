using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour
{
    private AudioSource audioSource;
    private bool finished = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;

        // Deteksi apakah objek atau parent-nya memiliki skrip PlayerController (roda/bodi mobil)
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            finished = true;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }

            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }

            StartCoroutine(LevelComplete());
        }
    }

    IEnumerator LevelComplete()
    {
        // Menunggu delay singkat (1 detik) setelah menyentuh finish agar terasa natural
        yield return new WaitForSeconds(1f);

        Debug.Log("LEVEL COMPLETE!");

        if (GameUIManager.instance != null)
        {
            GameUIManager.instance.ShowVictory(GameManager.instance.GetScore());
        }
    }
}