using UnityEngine;

public class Ball : MonoBehaviour
{


    public int ballType;

    public bool hasMerged = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMerged)
            return;

        Ball otherBall = collision.gameObject.GetComponent<Ball>();

        if (otherBall != null && !otherBall.hasMerged && otherBall.ballType == ballType)
        {
            hasMerged = true;
            otherBall.hasMerged = true;

            Vector3 mergePosition = (transform.position + otherBall.transform.position) / 2f;      // 두 과일의 중간 위치 계산

            // 게임 매니저에서 Merge 구현 된 것을 호출 (미구현)
            BallGame gameManager = FindAnyObjectByType<BallGame>();
            if (gameManager != null)
            {
                gameManager.MergeBalls(ballType, mergePosition);
            }

            // 과일들 제거

            Destroy(otherBall.gameObject);
            Destroy(gameObject);
        }
    }
}