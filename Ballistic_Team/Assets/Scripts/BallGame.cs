using UnityEngine;

public class BallGame : MonoBehaviour
{
    public GameObject[] ballPrefabs;
    public float[] ballSizes = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f };

    public GameObject currentBall;
    public int currentBallType;

    public float ballStartHeight = 6.0f;
    public bool isGameOver = false;
    //public Camera mainCamera;         Ä«¸Þ¶ó´Â BallThrow¿¡¼­ Ã³¸®ÇÒ ¿¹Á¤

    public float ballTimer;

    public Transform startPosition;
    
    public BallThrow ballThrow;

    public Vector2 throwInput;
    public float throwPower;

    public GameObject gameOverPannel;


    private void Awake()
    {
        ballThrow = FindAnyObjectByType<BallThrow>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //mainCamera = Camera.main;
        SpawnNewBall();
        ballTimer = -3.0f;
    }

    // Update is called once per frame
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
/*
        if (currentBall != null)
        {

            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 newPosition = currentBall.transform.position;
            newPosition.x = worldPosition.x;
            newPosition.y = worldPosition.y;

            float halfBallSize = ballSizes[currentBallType] / 2f;

            //if (newPosition.x < -gameWidth / 2 + halfBallSize)
            //{
            //    newPosition.x = -gameWidth / 2 + halfBallSize;
            //}
            //
            //if (newPosition.x > gameWidth / 2 + halfBallSize)
            //{
            //    newPosition.x = gameWidth / 2 + halfBallSize;
            //}

            currentBall.transform.position = newPosition;
        }
*/

    }
    
    
    // private float getfloat() => 
    private void OnDrawGizmosSelected()
    {

    }

    void SpawnNewBall()         // ±âÁ¸ °úÀÏ°ÔÀÓ ÄÚµå -> ÇÏÇÁ»çÀÌÁî °°Àº°Å ´Ù Áý¾îÄ¡¿ì°í ´Ù ¹Ù²Ü ¿¹Á¤ (¾ÆÁ÷Àº À¯Áö)
    {
        if (!isGameOver)
        {
            currentBallType = UnityEngine.Random.Range(0, 3);
            /*
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);       // ¸¶¿ì½º 2D À§Ä¡¸¦ ¿ùµå 3D ÁÂÇ¥·Î º¯È¯
            */
            // ±âÁ¸ ½ºÆù À§Ä¡ ÄÚµå
            //Vector3 spawnPosition = new Vector3(worldPosition.x, ballStartHeight, 0);
            Vector3 spawnPosition = startPosition.position;

            float halfFruitSize = ballSizes[currentBallType] / 2f;

            //spawnPosition.x = Mathf.Clamp(spawnPosition.x, -gameWidth / 2 + halfFruitSize, gameWidth / 2 - halfFruitSize);

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
        if(ballType < ballPrefabs.Length-1)
        {
            GameObject newBall = Instantiate(ballPrefabs[ballType+1], position, Quaternion.identity);
            newBall.transform.localScale = new Vector3(ballSizes[ballType + 1], ballSizes[ballType + 1], 1.0f);

            FindAnyObjectByType<AudioManager>().PlaySFX(FindAnyObjectByType<AudioManager>().BallMerge); // audio
            ScoreManager.instance.AddScoreByBallType(ballType + 1); // 합쳐질 때 +점수
        }
    }

    public void DropBall()
    {
        Debug.Log("°ø ´øÁ®");
        Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1f;
            rb.AddForce(throwInput * throwPower, ForceMode2D.Impulse);

            FindAnyObjectByType<AudioManager>().PlaySFX(FindAnyObjectByType<AudioManager>().BallShoot); // audio
            ScoreManager.instance.AddScoreByBallType(currentBallType); // 던질 때 +점수

            currentBall = null;
            ballTimer = 1.0f;
        }
    }
}