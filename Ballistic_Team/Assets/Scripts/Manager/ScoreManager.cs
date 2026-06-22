using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TMP_Text scoreText;
    private int score = 0;

    // 볼 타입(0~10) 점수
    public int[] scoreTable = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 550, 660 };

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateScoreText();
    }

    public void AddScoreByBallType(int ballType)
    {
        if (ballType < 0 || ballType >= scoreTable.Length)
        {
            Debug.LogWarning("ScoreManager: ballType 범위 초과 - " + ballType);
            return;
        }

        score += scoreTable[ballType];
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score : " + score;
    }
}