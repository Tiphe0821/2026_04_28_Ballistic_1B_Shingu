using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ball"))
        {
            Debug.Log("게임오버!");
            FindAnyObjectByType<BallGame>().isGameOver = true;
            Invoke("BackToLobby", 3f);
        }
    }

    private void BackToLobby()
    {
        SceneManager.LoadScene("UIScene");
    }
}
