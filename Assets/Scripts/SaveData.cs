using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SaveData : MonoBehaviour
{
    public class IndividualSave
    {
        public string Name = "name";
        public string HighestScore = "string";
    }
    public class Ranking {
        public string ScoreNumber;
        public string Name;
    }
    class RankingSave
    {
        public string[] Ranking = {
        ("Ranking_1st_place"),
        ("Ranking_2st_place"),
        ("Ranking_3st_place"),
        ("Ranking_4st_place"),
        ("Ranking_5st_place"),
        ("Ranking_6st_place"),
        ("Ranking_7st_place"),
        ("Ranking_8st_place"),
        ("Ranking_9st_place"),
        ("Ranking_10st_place"),
         };
        public string[] RankingName = {
        "RankingName_1st_place",
        "RankingName_2st_place",
        "RankingName_3st_place",
        "RankingName_4st_place",
        "RankingName_5st_place",
        "RankingName_6st_place",
        "RankingName_7st_place",
        "RankingName_8st_place",
        "RankingName_9st_place",
        "RankingName_10st_place",
         };
    }
    IndividualSave individualSave = new IndividualSave();
    RankingSave rankingSave = new RankingSave();
    public class Score
    {
        public int ScoreNumber;
        public string Name;
    }
    public void RankingFunction(int score ,string name) {
        Score[] RScore = new Score[11];
        RScore[10] = new Score() { ScoreNumber = score, Name = name };
        for (int y = 0; y < rankingSave.Ranking.Length; y++)
        {
            RScore[y] = new Score() { ScoreNumber = PlayerPrefs.GetInt(rankingSave.Ranking[y]), Name = PlayerPrefs.GetString(rankingSave.RankingName[y]) };
        }
        Score[] RankingScore = RScore.Where(e => e.ScoreNumber != 0).OrderBy(e => e.ScoreNumber).ToArray();
        for (int y = 0; y < rankingSave.Ranking.Length; y++)
        {
            if (RankingScore[y].ScoreNumber == PlayerPrefs.GetInt(rankingSave.Ranking[y])) return;
            PlayerPrefs.SetInt(rankingSave.Ranking[y], RankingScore[y].ScoreNumber);
            PlayerPrefs.SetString(rankingSave.RankingName[y], RankingScore[y].Name);
        }
    }
    public void HighestScore(int Sore) {
        int HighestScore = PlayerPrefs.GetInt(individualSave.HighestScore);
        if (Sore < HighestScore) return;
        PlayerPrefs.SetInt(individualSave.HighestScore, Sore);
    }
    public void Name(string name) {
        PlayerPrefs.SetString(individualSave.Name, name);
    }
        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
