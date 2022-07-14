using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject gameOverPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetScore(int score)
    {
        scoreText.text = "SCORE: " + score.ToString();
    }

    public void SetTime(int time)
    {
        timeText.text = "TIME LEFT: " + time.ToString();
    }

    public void GameOverPanel(bool flag)
    {
        gameOverPanel.SetActive(flag);
    }
}
