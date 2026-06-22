using UnityEngine;
using TMPro;

public class RankPage : MonoBehaviour
{
    public Transform contentParent;     // Scroll View의 Content 등
    public GameObject rankRowPrefab;    // 한 줄 프리팹 (닉네임 + 점수)

    private void OnEnable()
    {
        DisplayRanking();
    }

    private void DisplayRanking()
    {
        // 기존에 쌓여있던 UI 목록 삭제
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        var records = HighScore.GetTopRecords();

        for (int i = 0; i < records.Count; i++)
        {
            // 랭킹 생성
            GameObject row = Instantiate(rankRowPrefab, contentParent);

            // 복사된 오브젝트 자체에서 혹은 자식에게서 TMP_Text 컴포넌트를 가져옴
            TMP_Text textComponent = row.GetComponentInChildren<TMP_Text>();

            if (textComponent != null)
            {
                // 한 줄에 순위, 닉네임, 점수를 한 번에 표현
                textComponent.text = $"{i + 1}. {records[i].nickname} : {records[i].score}";
            }
            else
            {
                Debug.LogError("Rank Row Prefab에 TextMeshPro 컴포넌트가 존재하지 않습니다!");
            }
        }
    }
}