using UnityEngine;

public class Coin : MonoBehaviour
{
    public int score = 10;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private bool collected = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        // Deteksi apakah objek yang menyentuh koin memiliki skrip PlayerController (misalnya roda)
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            collected = true;

            GameManager.instance.AddScore(score);

            spriteRenderer.enabled = false;
            col.enabled = false;

            float destroyDelay = 0.1f;
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
                destroyDelay = audioSource.clip.length;
            }

            Destroy(gameObject, destroyDelay);
        }
    }
}