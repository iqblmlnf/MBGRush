using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        // Pola Singleton + DontDestroyOnLoad agar musik tidak terputus saat pindah scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Memastikan musik langsung diputar dan meloop
        if (audioSource != null && audioSource.clip != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.playOnAwake = true;
                audioSource.Play();
            }
        }
    }
}
