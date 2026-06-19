using UnityEngine;


public class BallThrow : MonoBehaviour
{
    public GameObject currentBall;
    public BallGame myGame;

    public Transform startPosition;
    private float maxminDistance = 0.5f;
    public Camera mainCamera;

    public Transform HandPos;
    public GameObject PowerBar;
    public SpriteRenderer barSprite;


    public float throwForce;

    private Vector2 playerDir;

    public bool isGrab = false;

    // 마우스 클릭 시 해당 위치 저장하는 변수
    public Vector2 startHoldPosition;
    private Vector2 throwVector;            // 던지는 방향 벡터 (


    void Start()
    {
        mainCamera = Camera.main;
        myGame = FindAnyObjectByType<BallGame>();   
    }

    void Update()
    {
        // 볼 게임에서 코드 옮겨오기
        Vector3 mousePosition = Input.mousePosition;        // 마우스 포지션
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);   // 월드 포지션으로 변환

        playerDir = new Vector2(worldPosition.x - this.transform.position.x, worldPosition.y - this.transform.position.y);
        // 던지는 손이 플레이어의 마우스 포지션을 보도록 만들기
        float lookAngle = Mathf.Acos((playerDir.normalized.x * playerDir.normalized.x) / (Mathf.Abs(playerDir.normalized.magnitude) * Mathf.Abs(playerDir.normalized.x)));
        bool isLookUp = false;

        if (playerDir.y > 0)
            isLookUp = true;
        if (playerDir.x > 0)
            HandPos.localScale = new Vector3(1, 1, 1);
        else
            HandPos.localScale = new Vector3(-1, 1, 1);


        if (playerDir.x > 0)
        {
            HandPos.rotation = Quaternion.Euler(0f, 0f, ((isLookUp) ? 1.0f : -1.0f) * Mathf.Rad2Deg * lookAngle);
            PowerBar.transform.rotation = Quaternion.Euler(0f, 0f, ((isLookUp) ? 1.0f : -1.0f) * Mathf.Rad2Deg * lookAngle);
        }
        else
        {
            HandPos.rotation = Quaternion.Euler(0f, 0f, -((isLookUp) ? 1.0f : -1.0f) * Mathf.Rad2Deg * lookAngle);
            PowerBar.transform.rotation = Quaternion.Euler(0f, 0f, -((isLookUp) ? 1.0f : -1.0f) * Mathf.Rad2Deg * lookAngle);
        }

        if(isGrab &&  currentBall != null )
        {
            // 현재 마우스 포지션 가져와서 처음 클릭한 위치랑 차이 계산하기
            throwVector = new Vector2(worldPosition.x - startHoldPosition.x, worldPosition.y - startHoldPosition.y);

            PowerBar.transform.localScale = new Vector3(Mathf.Clamp(throwVector.magnitude, 0.1f, 1) * 2, 1f, 1f);
            if (throwVector.magnitude < 0.5f)
            {
                barSprite.color = Color.black;
            }
            else
            {
                barSprite.color = Color.white;
            }
            /*
            Vector3 newPosition = currentBall.transform.position;
            newPosition.x = worldPosition.x;
            newPosition.y = worldPosition.y;

            // 하고싶은거 : 공이 마우스를 따라 움직이되 원형 범위 밖으로 나가지 못하도록 만드는 것
            // Clamp를 활용해보자
            // 삼각함수? -> 사용이 가능할까..? (코사인, 사인 활용하면 가능할 것 같다. 근데 어떻게?)
            // 일단 던지기 구현에 집중하자 - 나중에 시간 남으면 구현 시도

            newPosition.x = Mathf.Clamp(newPosition.x, startPosition.position.x - maxminDistance, startPosition.position.x + maxminDistance);
            newPosition.y = Mathf.Clamp(newPosition.y, startPosition.position.y - maxminDistance, startPosition.position.y + maxminDistance);

            currentBall.transform.position = newPosition;
            */
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(!isGrab)
            {
                BallGrab();
                PowerBar.SetActive(true);
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            PowerBar.SetActive(false);

            if(isGrab && throwVector.magnitude > 0.5f)      // 던지는 힘이 0.5(절반)보다 강할 경우에만 던지도록 만들기
            {
                myGame.DropBall();
                BallRelease();
            }
            else if(isGrab)
            {
                isGrab = false;
            }
        }

    }
    private void DrawThrowLine()
    {
        
    }

    private void BallGrab()         // 공 잡기
    {
        isGrab = true;
        Vector3 mousePosition = Input.mousePosition;        // 마우스 포지션
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);   // 월드 포지션으로 변환

        startHoldPosition = new Vector2(worldPosition.x, worldPosition.y);
        // 이걸 기준으로 마우스 위치를 입력받아서 던질 예정

    }

    private void BallRelease()      // 공 던지기
    {
        isGrab = false;

        float howStrong = Mathf.Clamp(throwVector.magnitude, 0.1f, 1.0f) * throwForce;

        currentBall.GetComponent<Rigidbody2D>().AddForce(-playerDir.normalized * howStrong, ForceMode2D.Impulse);
    }
}
