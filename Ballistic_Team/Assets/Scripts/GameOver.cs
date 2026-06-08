using UnityEngine;

public class GameOver : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ball"))
        {
            Debug.Log("게임오버!");
            FindAnyObjectByType<BallGame>().isGameOver = true;
        }
    }
}
