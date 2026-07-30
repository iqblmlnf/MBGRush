using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TMP_Text scoreText;

    private int score;

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
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score : " + score;
    }

    public int GetScore()
    {
        return score;
    }
}