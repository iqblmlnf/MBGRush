using System.Collections.Generic;
using UnityEngine;

public class EndlessTrackManager : MonoBehaviour
{
    public static EndlessTrackManager instance;

    [Header("Target References")]
    [Tooltip("Transform mobil/pemain yang diikuti")]
    public Transform player;

    [Header("Chunk Settings")]
    [Tooltip("Daftar prefab potongan jalur (Chunk) yang di-spawn secara acak")]
    public GameObject[] trackChunkPrefabs;

    [Tooltip("Prefab chunk khusus untuk area awal (opsional, misal jalur datar tanpa rintangan)")]
    public GameObject firstChunkPrefab;

    [Tooltip("Panjang fisik 1 prefab chunk dalam satuan unit/meter Unity")]
    public float chunkLength = 40f;

    [Tooltip("Jumlah chunk yang selalu aktif berada di depan pemain")]
    public int chunksOnScreen = 4;

    [Tooltip("Jarak aman di belakang pemain sebelum chunk lama dihapus")]
    public float destroyDistance = 60f;

    private float spawnX = 0f;
    private List<GameObject> activeChunks = new List<GameObject>();

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
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
            {
                player = p.transform;
            }
        }

        // Simpan posisi awal X player jika player terpasang
        if (player != null)
        {
            spawnX = player.position.x - 5f; // Beri sedikit offset ke belakang agar mobil tidak mengambang di awal
        }

        // Spawn chunk pertama
        if (firstChunkPrefab != null)
        {
            SpawnChunk(firstChunkPrefab);
        }
        else if (trackChunkPrefabs != null && trackChunkPrefabs.Length > 0)
        {
            SpawnChunk(trackChunkPrefabs[0]);
        }

        // Pre-spawn chunk berikutnya di depan
        for (int i = 1; i < chunksOnScreen; i++)
        {
            SpawnRandomChunk();
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Cek apakah posisi player sudah mendekati batas spawnX
        if (player.position.x + (chunksOnScreen * chunkLength / 2f) > spawnX)
        {
            SpawnRandomChunk();
        }

        // Hapus chunk paling belakang yang sudah tidak terlihat
        RemoveOldestChunk();
    }

    private void SpawnRandomChunk()
    {
        if (trackChunkPrefabs == null || trackChunkPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, trackChunkPrefabs.Length);
        SpawnChunk(trackChunkPrefabs[randomIndex]);
    }

    private void SpawnChunk(GameObject prefab)
    {
        if (prefab == null) return;

        GameObject chunk = Instantiate(prefab, new Vector3(spawnX, 0f, 0f), Quaternion.identity, transform);
        activeChunks.Add(chunk);
        spawnX += chunkLength;
    }

    private void RemoveOldestChunk()
    {
        if (activeChunks.Count > 0)
        {
            GameObject oldestChunk = activeChunks[0];
            if (oldestChunk != null && player.position.x - oldestChunk.transform.position.x > destroyDistance + chunkLength)
            {
                activeChunks.RemoveAt(0);
                Destroy(oldestChunk);
            }
        }
    }
}
