using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public int maxEntries;
    public Transform entryContainer, entryTemplate;
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    public List<Transform> leaderboardEntryTransformList = new List<Transform>();

    private void Awake(){

        entries.Add(new LeaderboardEntry("MAY", 69420.0f, 999));
        entries.Add(new LeaderboardEntry("WOD", 69419.0f, 997));
        entries.Add(new LeaderboardEntry("DND", 69.42f, 22));
        entries.Add(new LeaderboardEntry("RNG", 420.69f, 152));
        entries.Add(new LeaderboardEntry("UFO", 6.924f, 362));
        entries.Add(new LeaderboardEntry("AYY", 69.5f, 174));

        entryContainer = transform.Find("Score Container");
        entryTemplate = entryContainer.Find("Score Template");

        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("Leaderboard");
        Leaderboard leaderboard = new Leaderboard(maxEntries, entries); //JsonUtility.FromJson<Leaderboard>(jsonString);

        foreach (LeaderboardEntry entry in leaderboard.leaderboardEntry){
            CreateEntryTransform(entry, entryContainer, leaderboardEntryTransformList);
        }

        string json = JsonUtility.ToJson(entries);
        PlayerPrefs.SetString("Leaderboard", json);
        PlayerPrefs.Save();
    }

    private void CreateEntryTransform(LeaderboardEntry entry, Transform entryContainer, List<Transform> transformList){
        float templateHeight = 20f;
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);
        entryTransform.Find("Name").GetComponent<TMP_Text>().text = entry.name;
        TimeSpan time = TimeSpan.FromSeconds(entry.seconds);
        entryTransform.Find("Time").GetComponent<TMP_Text>().text = time.ToString();
        entryTransform.Find("Score").GetComponent<TMP_Text>().text = entry.score.ToString();
    }

    [Serializable]
    public class LeaderboardEntry { 
        public string name;
        public float seconds;
        public int score;

        public LeaderboardEntry(string name, float seconds, int score){
            this.name = name;
            this.seconds = seconds;
            this.score = score;
        }
    }

    public class Leaderboard {
        public int maxEntries;
        public List<LeaderboardEntry> leaderboardEntry;

        public Leaderboard(int max){
            this.maxEntries = max;
            this.leaderboardEntry = new List<LeaderboardEntry>();
        }

        public Leaderboard(int max, List<LeaderboardEntry> entries){
            this.maxEntries = max;
            this.leaderboardEntry = entries;
            while(entries.Count > maxEntries){
                entries.RemoveAt(entries.Count-1);
            }
        }
    }
}
