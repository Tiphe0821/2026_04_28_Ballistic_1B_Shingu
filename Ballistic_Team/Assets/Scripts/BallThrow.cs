using UnityEngine;

public class BallThrow : MonoBehaviour
{
    public GameObject currentBall;

    public Rigidbody2D currentRb;
    public Transform startPosition;
    private float maxminDistance = 2f;
    public Camera mainCamera;

    public bool isGrab = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(isGrab &&  currentBall != null )
        {
            // 볼 게임에서 코드 옮겨오기
            Vector3 mousePosition = Input.mousePosition;        // 마우스 포지션
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);   // 월드 포지션으로 변환

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
        }
    }

    private void DrawThrowLine()
    {
        
    }

    private void BallGrab()         // 공 잡기
    {
        isGrab = true;
    }

    private void BallRelease()      // 공 던지기
    {

    }
}
