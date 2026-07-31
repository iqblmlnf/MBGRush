using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Text References")]
    public TMP_Text scoreText;
    public TMP_Text distanceText;

    [Header("Player Tracking")]
    public Transform playerTransform;

    private int score;
    private float startX;
    private float maxDistance = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : 0";
        }

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        if (playerTransform != null)
        {
            startX = playerTransform.position.x;
        }

        UpdateDistanceUI();
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float currentDistance = Mathf.Max(0f, playerTransform.position.x - startX);
            if (currentDistance > maxDistance)
            {
                maxDistance = currentDistance;
                UpdateDistanceUI();
            }
        }
    }

    private void UpdateDistanceUI()
    {
        if (distanceText != null)
        {
            distanceText.text = Mathf.FloorToInt(maxDistance) + " m";
        }
    }

    public void AddScore(int value)
    {
        score += value;
        if (scoreText != null)
        {
            scoreText.text = "Score : " + score;
        }
    }

    public int GetScore()
    {
        return score;
    }

    public int GetDistance()
    {
        return Mathf.FloorToInt(maxDistance);
    }
}