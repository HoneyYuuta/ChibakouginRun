using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージと難易度管理クラス
public class StageController
{
    private int stageIndex;
    private int difficultyLevel;

    private ItemDataBase stageDB;
    private ObjectProbabilityDatabase probabilityDB;

    public GameObject Floor { get; private set; }
    public GameObject Obstacles { get; private set; }
    public GameObject Items { get; private set; }
    public GameObject Connection { get; private set; }
    public string StageName { get; private set; }

    public WeightedItem[] items = new WeightedItem[4];

    public void Add()
    {
        items[0] = new WeightedItem();
        items[0].segmentType = SegmentType.Safe;
        items[0].weight = 50;
        items[1] = new WeightedItem();
        items[1].segmentType = SegmentType.Single;
        items[1].weight = 50;
        items[2] = new WeightedItem();
        items[2].segmentType = SegmentType.Shift;
        items[2].weight = 50;
        items[3] = new WeightedItem();
        items[3].segmentType = SegmentType.Double;
        items[3].weight = 50;
    }

    public StageController(ItemDataBase db, ObjectProbabilityDatabase prob)
    {
        stageDB = db;
        probabilityDB = prob;
        Add();
    }

    public void NextStage()
    {
        DifficultyUP();
        if (stageDB.ItemList.Count <= stageIndex)
            stageIndex = 0;
        var data = stageDB.ItemList[stageIndex];
        Floor = data.FloorObject;
        Obstacles = data.ObstaclesObject;
        Items = data.ItemObject;
        Connection = data.ConnectionObject;
        StageName = data.stageName;
        stageIndex++;
    }
    // 難易度を上げるメソッド
    void DifficultyUP() { 
        if(probabilityDB == null) return;
        if(probabilityDB.ItemList.Count <= difficultyLevel) return;
        items[0].weight = probabilityDB.ItemList[difficultyLevel].SkyProbability;
        items[1].weight = probabilityDB.ItemList[difficultyLevel].ObstacleProbability;
        items[2].weight = probabilityDB.ItemList[difficultyLevel].ItemProbability;
        items[3].weight = probabilityDB.ItemList[difficultyLevel].ProbabilityOf2Obstacles;
        difficultyLevel++;
    }
}