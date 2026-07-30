using UnityEngine;

public class FuelCan : MonoBehaviour
{
    [Header("Fuel Settings")]
    [Tooltip("Jumlah bensin yang diisi ulang (0 - 100)")]
    public float fuelRestoreAmount = 30f;

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

        // Deteksi apakah objek yang menyentuh bensin memiliki skrip PlayerController (misalnya roda/bodi)
        PlayerController player = other.GetComponentInParent<PlayerController>();
        
        if (player != null)
        {
            collected = true;

            // Isi ulang bensin pemain
            player.RestoreFuel(fuelRestoreAmount);

            // Sembunyikan item agar tidak terlihat lagi tapi tetap bisa memutar suara
            spriteRenderer.enabled = false;
            col.enabled = false;

            float destroyDelay = 0.1f;
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
                destroyDelay = audioSource.clip.length;
            }

            // Hapus objek bensin setelah suara selesai diputar
            Destroy(gameObject, destroyDelay);
        }
    }
}
