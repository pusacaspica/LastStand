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

        /*entries.Add(new LeaderboardEntry("MAY", 69420.0f, 999));
        entries.Add(new LeaderboardEntry("WOD", 69419.0f, 997));
        entries.Add(new LeaderboardEntry("UFO", 6.924f, 362));
        entries.Add(new LeaderboardEntry("AYY", 69.5f, 174));
        entries.Add(new LeaderboardEntry("RNG", 420.69f, 152));
        entries.Add(new LeaderboardEntry("DND", 69.42f, 22));*/

        entryContainer = transform.Find("Score Container");
        entryTemplate = entryContainer.Find("Score Template");

        entryTemplate.gameObject.SetActive(false);

        AddLeaderboardEntry("LHM", 256.0f, 998);

        string jsonString = PlayerPrefs.GetString("Leaderboard");
        Leaderboard leaderboard = JsonUtility.FromJson<Leaderboard>(jsonString);

        foreach (LeaderboardEntry entry in leaderboard.leaderboardEntries){
            CreateEntryTransform(entry, entryContainer, leaderboardEntryTransformList);
        }

        string json = JsonUtility.ToJson(leaderboard);
        PlayerPrefs.SetString("Leaderboard", json);
        PlayerPrefs.Save();

        Debug.Log(PlayerPrefs.GetString("Leaderboard"));
    }

    public void AddLeaderboardEntry(string name, float seconds, int score){
        //Load leaderboard
        string jsonString = PlayerPrefs.GetString("Leaderboard");
        Leaderboard leaderboard = JsonUtility.FromJson<Leaderboard>(jsonString);

        //if score was lesser than the last score in leaderboard, doesn't even consider
        if(score < leaderboard.leaderboardEntries[leaderboard.leaderboardEntries.Count - 1].score) return;
        LeaderboardEntry entry = new LeaderboardEntry(name, seconds, score);

        //adds score on leaderboard, puts it in the right place
        leaderboard.leaderboardEntries.Add(entry);
        for(int i = 0; i < leaderboard.leaderboardEntries.Count; i++){
            for(int j = i+1; j < leaderboard.leaderboardEntries.Count; j++){
                if(leaderboard.leaderboardEntries[j].score > leaderboard.leaderboardEntries[i].score){
                    LeaderboardEntry tmp = leaderboard.leaderboardEntries[i];
                    leaderboard.leaderboardEntries[i] = leaderboard.leaderboardEntries[j];
                    leaderboard.leaderboardEntries[j] = tmp;
                }
            }
        }
        if(leaderboard.leaderboardEntries.Count > maxEntries){
            leaderboard.leaderboardEntries.RemoveAt(leaderboard.leaderboardEntries.Count - 1);
        }

        string json = JsonUtility.ToJson(leaderboard);
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

        transformList.Add(entryTransform);
    }

    [System.Serializable]
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

    [System.Serializable]
    public class Leaderboard {
        public int maxEntries;
        public List<LeaderboardEntry> leaderboardEntries;

        public Leaderboard(int max){
            this.maxEntries = max;
            this.leaderboardEntries = new List<LeaderboardEntry>();
        }

        public Leaderboard(int max, List<LeaderboardEntry> entries){
            this.maxEntries = max;
            this.leaderboardEntries = entries;
            while(entries.Count > maxEntries){
                entries.RemoveAt(entries.Count-1);
            }
        }
    }
}
