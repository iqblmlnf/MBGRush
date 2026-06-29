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

        if (collision.gameObject.CompareTag("Player"))
        {
            crashed = true;

            audioSource.Play();

            StartCoroutine(RestartScene());
        }
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}