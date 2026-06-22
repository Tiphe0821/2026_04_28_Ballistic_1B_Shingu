using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{

    public GameObject GameOverPannel;

    // 점수 + 닉네임 기록 
    [Header("Game Over Panel Texts")]
    //public TMP_Text finalNicknameText;
    //public TMP_Text finalScoreText;
    public TMP_Text finalResultText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            Debug.Log("°ÔÀÓ¿À¹ö!");
            FindAnyObjectByType<BallGame>().isGameOver = true;

            GameOverPannel.SetActive(true);

            // [추가] 게임오버 창이 뜨는 순간, 현재 판의 점수와 이름을 화면에 표기해줍니다.
            string nickname = PlayerPrefs.GetString("PlayerName", "Guest");
            int finalScore = ScoreManager.instance.GetScore();

            if (finalResultText != null)
            {
                finalResultText.text += $"\n{nickname} : {finalScore}";
            }

            //if (finalNicknameText != null) finalNicknameText.text = $"{nickname}";
            //if (finalScoreText != null) finalScoreText.text = $"{finalScore}";
        }
    }

    public void BackToLobby()
    {
        SaveScore(); // 점수 저장
        SceneManager.LoadScene("UIScene");
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void SaveScore()
    {
        string nickname = PlayerPrefs.GetString("PlayerName", "Guest");
        int finalScore = ScoreManager.instance.GetScore();

        HighScore.AddRecord(nickname, finalScore);
        Debug.Log("저장 시도");
        Debug.Log($"랭킹 저장 완료: {nickname} - {finalScore}");
    }
}

