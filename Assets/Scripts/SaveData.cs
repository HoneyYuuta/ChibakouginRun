using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public static class SaveData
{
    public static class IndividualSave
    {
        public static string Name = "name";
        public static string HighestScore = "string";
    }
    public class Ranking {
        public string ScoreNumber;
        public string Name;
    }
    public static class  RankingSave
    {
        public static string[] Ranking = {
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
        public static string[] RankingName = {
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

    public class Score
    {
        public int ScoreNumber;
        public string Name;
    }

    //ランキング機能
    public static void RankingFunction(int score ,string name) {
        if(score <= PlayerPrefs.GetInt(RankingSave.Ranking[RankingSave.Ranking.Length -1])) return;
        Score[] RScore = new Score[11];
        RScore[10] = new Score() { ScoreNumber = score, Name = name };
        for (int y = 0; y < RankingSave.Ranking.Length; y++)
        {
            RScore[y] = new Score() { ScoreNumber = PlayerPrefs.GetInt(RankingSave.Ranking[y]), Name = PlayerPrefs.GetString(RankingSave.RankingName[y]) };
        }
        Score[] RankingScore = RScore.Where(e => e.ScoreNumber != 0).OrderBy(e => e.ScoreNumber).ToArray();
        for (int y = 0; y < RankingSave.Ranking.Length; y++)
        {
            if (RankingScore[y].ScoreNumber == PlayerPrefs.GetInt(RankingSave.Ranking[y])) return;
            PlayerPrefs.SetInt(RankingSave.Ranking[y], RankingScore[y].ScoreNumber);
            PlayerPrefs.SetString(RankingSave.RankingName[y], RankingScore[y].Name);
        }
    }
    //個人データ機能
    public static void HighestScore(int Sore) {
        int HighestScore = PlayerPrefs.GetInt(IndividualSave.HighestScore);
        if (Sore < HighestScore) return;
        PlayerPrefs.SetInt(IndividualSave.HighestScore, Sore);
    }
    public static void Name(string name) {
        PlayerPrefs.SetString(IndividualSave.Name, name);
    }
    //名前
    public static string SetName()
    {
        return PlayerPrefs.GetString(IndividualSave.Name, "Player");
    }
    //最高得点
    public static int SetHighestScore()
    {
        return PlayerPrefs.GetInt(IndividualSave.HighestScore, 0);
    }
    //ランキング得点
    public static int SetRankingScore(int rank)
    {
        return PlayerPrefs.GetInt(RankingSave.Ranking[rank], 0);
    }
    //ランキング名前
    public static string SetRankingName(int rank)
    {
        return PlayerPrefs.GetString(RankingSave.RankingName[rank]);
    }

}
