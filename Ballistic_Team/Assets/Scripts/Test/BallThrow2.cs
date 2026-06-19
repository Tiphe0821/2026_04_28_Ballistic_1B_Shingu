using UnityEngine;

public class BallThrow2 : MonoBehaviour
{
    public GameObject currentBall;
    public BallGame2 myGame;

    public Transform startPosition;
    private float maxminDistance = 0.5f;
    public Camera mainCamera;

    public Transform HandPos;

    // --- 궤적(Trajectory) 그리기용 변수 추가 ---
    [Header("Trajectory Settings")]
    public GameObject dotPrefab;        // 하얀 점 프리팹 (Inspector에서 할당)
    public int numberOfDots = 15;       // 표시할 점의 개수
    public float dotSpacing = 0.05f;    // 점들 사이의 시간 간격 (간격이 넓어질수록 궤적을 멀리 보여줌)
    [Range(0.1f, 3f)]
    public float fadeSpeed = 1.0f;      // 끝으로 갈수록 투명해지는 정도 조절

    private GameObject[] dots;          // 생성된 점들을 담아둘 배열
    private SpriteRenderer[] dotRenderers; // 투명도 조절을 위한 렌더러 캐싱

    public float throwForce;
    private Vector2 playerDir;
    public bool isGrab = false;

    // 마우스 클릭 시 해당 위치 저장하는 변수
    public Vector2 startHoldPosition;
    private Vector2 throwVector;            // 던지는 방향 벡터


    void Start()
    {
        mainCamera = Camera.main;
        myGame = FindAnyObjectByType<BallGame2>();

        // 시작할 때 궤적용 점들을 미리 생성해서 숨겨둠 (오브젝트 풀링 방식)
        dots = new GameObject[numberOfDots];
        dotRenderers = new SpriteRenderer[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, transform.position, Quaternion.identity);
            dotRenderers[i] = dots[i].GetComponent<SpriteRenderer>();
            dots[i].SetActive(false);
        }
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;        // 마우스 포지션
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);   // 월드 포지션으로 변환

        playerDir = new Vector2(worldPosition.x - this.transform.position.x, worldPosition.y - this.transform.position.y);

        // 던지는 손이 플레이어의 마우스 포지션을 보도록 만들기
        float lookAngle = Mathf.Acos((playerDir.normalized.x * playerDir.normalized.x) / (Mathf.Abs(playerDir.normalized.magnitude) * Mathf.Abs(playerDir.normalized.x)));
        bool isLookUp = playerDir.y > 0;

        if (playerDir.x > 0)
            HandPos.localScale = new Vector3(1, 1, 1);
        else
            HandPos.localScale = new Vector3(-1, 1, 1);


        if (playerDir.x > 0)
        {
            HandPos.rotation = Quaternion.Euler(0f, 0f, ((isLookUp) ? 1.0f : -1.0f) * Mathf.Rad2Deg * lookAngle);
        }
        else
        {
            HandPos.rotation = Quaternion.Euler(0f, 0f, -((isLookUp) ? 1.0f : -1.0f) * Mathf.Rad2Deg * lookAngle);
        }

        if (isGrab && currentBall != null)
        {
            // 현재 마우스 포지션 가져와서 처음 클릭한 위치랑 차이 계산하기
            throwVector = new Vector2(worldPosition.x - startHoldPosition.x, worldPosition.y - startHoldPosition.y);

            // 힘이 일정 수치 이상일 때만 궤적 보여주기
            if (throwVector.magnitude >= 0.5f)
            {
                DrawTrajectory();
            }
            else
            {
                HideTrajectory();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isGrab)
            {
                BallGrab();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            HideTrajectory(); // 마우스를 떼면 궤적 숨기기

            if (isGrab && throwVector.magnitude > 0.5f)      // 던지는 힘이 0.5(절반)보다 강할 경우에만 던지도록 만들기
            {
                myGame.DropBall();
                BallRelease();
            }
            else if (isGrab)
            {
                isGrab = false;
            }
        }
    }

    private void DrawTrajectory()
    {
        // 1. 공을 던질 때 가해지는 힘 계산 (BallRelease와 동일한 계산식 적용)
        float howStrong = Mathf.Clamp(throwVector.magnitude, 0.1f, 1.0f) * throwForce;
        Vector2 force = -playerDir.normalized * howStrong;

        Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();

        // 2. 초기 속도 계산 (Impulse = 질량 * 속도 변화량)
        Vector2 initialVelocity = force / rb.mass;
        Vector2 startPos = currentBall.transform.position;

        // 3. 중력값 설정 (게임 매니저에서 떨어질 때 gravityScale이 1이 되므로 Physics2D.gravity 사용)
        Vector2 gravity = Physics2D.gravity * 1f;

        // 4. 각 점(Dot)의 위치 계산 및 투명도 조절
        for (int i = 0; i < numberOfDots; i++)
        {
            float t = i * dotSpacing; // 각 점마다 흐른 시간

            // 포물선 위치 공식 적용
            Vector2 pointPosition = startPos + initialVelocity * t + 0.5f * gravity * t * t;

            dots[i].transform.position = pointPosition;
            dots[i].SetActive(true);

            // fadeSpeed에 따라 점이 점점 투명해지도록 알파값 조절
            Color c = dotRenderers[i].color;
            float alpha = 1f - ((float)i / numberOfDots) * fadeSpeed;
            c.a = Mathf.Clamp01(alpha); // 알파값이 0~1 사이를 벗어나지 않도록 고정
            dotRenderers[i].color = c;
        }
    }

    private void HideTrajectory()
    {
        // 모든 궤적 점 숨기기
        for (int i = 0; i < numberOfDots; i++)
        {
            if (dots[i] != null) dots[i].SetActive(false);
        }
    }

    private void BallGrab()         // 공 잡기
    {
        isGrab = true;
        Vector3 mousePosition = Input.mousePosition;        // 마우스 포지션
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);   // 월드 포지션으로 변환

        startHoldPosition = new Vector2(worldPosition.x, worldPosition.y);
    }

    private void BallRelease()      // 공 던지기
    {
        isGrab = false;

        float howStrong = Mathf.Clamp(throwVector.magnitude, 0.1f, 1.0f) * throwForce;
        currentBall.GetComponent<Rigidbody2D>().AddForce(-playerDir.normalized * howStrong, ForceMode2D.Impulse);
    }
}