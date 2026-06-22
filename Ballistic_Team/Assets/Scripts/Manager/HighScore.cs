using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ScoreEntry
{
    public string nickname;
    public int score;

    public ScoreEntry(string nickname, int score)
    {
        this.nickname = nickname;
        this.score = score;
    }
}

[System.Serializable]
public class ScoreEntryList
{
    public List<ScoreEntry> entries = new List<ScoreEntry>();
}

public static class HighScore
{
    private const string KEY = "RankingData";
    private const int MAX_ENTRIES = 10; // 몇 개까지 저장

    public static void AddRecord(string nickname, int score)
    {
        ScoreEntryList list = Load();

        list.entries.Add(new ScoreEntry(nickname, score));

        // 점수 내림차순
        list.entries = list.entries.OrderByDescending(e => e.score).ToList();

        // 상위 N개만 유지 (그 밑으로는 자동 삭제)
        if (list.entries.Count > MAX_ENTRIES)
        {
            list.entries = list.entries.Take(MAX_ENTRIES).ToList();
        }

        Save(list);
    }

    public static List<ScoreEntry> GetTopRecords()
    {
        return Load().entries;
    }

    private static ScoreEntryList Load()
    {
        string json = PlayerPrefs.GetString(KEY, "");
        if (string.IsNullOrEmpty(json))
            return new ScoreEntryList();

        return JsonUtility.FromJson<ScoreEntryList>(json);
    }

    private static void Save(ScoreEntryList list)
    {
        string json = JsonUtility.ToJson(list);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }
}