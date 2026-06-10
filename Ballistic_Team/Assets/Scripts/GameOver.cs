using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public GameObject GameOverPannel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ball"))
        {
            Debug.Log("게임오버!");
            FindAnyObjectByType<BallGame>().isGameOver = true;


        }
    }

    private void BackToLobby()
    {
        SceneManager.LoadScene("UIScene");
    }
}
