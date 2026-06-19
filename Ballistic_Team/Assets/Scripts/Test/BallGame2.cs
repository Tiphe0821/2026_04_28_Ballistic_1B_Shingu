using UnityEngine;

public class BallGame2 : MonoBehaviour
{
    public GameObject[] ballPrefabs;
    public float[] ballSizes = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f };

    public GameObject currentBall;
    public int currentBallType;

    public float ballStartHeight = 6.0f;
    public bool isGameOver = false;

    public float ballTimer;

    public Transform startPosition;

    // BallThrow2 로 참조 변경
    public BallThrow2 ballThrow;

    public Vector2 throwInput;
    public float throwPower;

    public GameObject gameOverPannel;


    private void Awake()
    {
        ballThrow = FindAnyObjectByType<BallThrow2>();
    }

    void Start()
    {
        SpawnNewBall();
        ballTimer = -3.0f;
    }

    void Update()
    {
        if (isGameOver)
            return;

        if (ballTimer >= 0)
        {
            ballTimer -= Time.deltaTime;
        }

        if (ballTimer < 0 && ballTimer > -2)
        {
            SpawnNewBall();
            ballTimer = -3.0f;
        }
    }

    private void OnDrawGizmosSelected()
    {

    }

    void SpawnNewBall()
    {
        if (!isGameOver)
        {
            currentBallType = UnityEngine.Random.Range(0, 3);

            Vector3 spawnPosition = startPosition.position;

            currentBall = Instantiate(ballPrefabs[currentBallType], spawnPosition, Quaternion.identity);
            currentBall.transform.localScale = new Vector3(ballSizes[currentBallType], ballSizes[currentBallType], 1);

            Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.gravityScale = 0f;
            }

            ballThrow.currentBall = currentBall;
        }
    }

    public void MergeBalls(int ballType, Vector3 position)
    {
        if (ballType < ballPrefabs.Length - 1)
        {
            GameObject newBall = Instantiate(ballPrefabs[ballType + 1], position, Quaternion.identity);
            newBall.transform.localScale = new Vector3(ballSizes[ballType + 1], ballSizes[ballType + 1], 1.0f);
        }
    }

    public void DropBall()
    {
        Debug.Log("공 던져");
        Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1f;

            rb.AddForce(throwInput * throwPower, ForceMode2D.Impulse);
            currentBall = null;
            ballTimer = 1.0f;
        }
    }
}