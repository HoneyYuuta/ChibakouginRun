using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEditor.Progress;

public class automaticFloor : MonoBehaviour
{
    GameObject floor;//床のオブジェクト
    GameObject Obstacles;//障害物のオブジェクト
    GameObject Items;//アイテムのオブジェクト
    [SerializeField]
    private ItemDataBase StageDat;//ステージデータベース
    [SerializeField]
    private ObjectProbabilityDatabase objectProbability;//オブジェクト確率データ
    int DifficultyLevel = 0;//難易度レベル
    [SerializeField] GameObject[] floorObject;//床オブジェクトの配列
    public int XCoordinate=0;//X座標
    [SerializeField] float ItemWidth = 1.5f;//アイテムの幅
    int StageDatabaseIndex = 0;//ステージデータベースのインデックス
    [SerializeField] int FrequencyOfStageChanges = 100;//ステージ変更の頻度
     int FrequencyOfObstacles = 40;//障害物の確率
     int ObjectAppearanceProbability = 30;//アイテムの確率
    int ProbabilityOf2Obstacles = 0;//2つの障害物の確率

    // Start is called before the first frame update
    void Start()
    {
        StageChange();
        DifficultyUP();
        for (int i = 0; i < 10; i++)
        {
            movingObject(floor, new Vector2(0, 0));
            XCoordinate++;
        }
    }

    void DifficultyUP() { 
    if(objectProbability == null) return;
    if(objectProbability.ItemList.Count <= DifficultyLevel) return;
        FrequencyOfObstacles = objectProbability.ItemList[DifficultyLevel].FrequencyOfObstacles;
        ObjectAppearanceProbability = objectProbability.ItemList[DifficultyLevel].ObjectAppearanceProbability;
        ProbabilityOf2Obstacles = objectProbability.ItemList[DifficultyLevel].ProbabilityOf2Obstacles;
        DifficultyLevel++;

    }
    //このメソッドは、現在のインデックス X に基づいて床、障害物、アイテムを更新してステージを変更します
    void StageChange()
    {
        if (StageDat == null) return;
        if (StageDat.ItemList.Count <= StageDatabaseIndex) StageDatabaseIndex = 0;
        floor = StageDat.ItemList[StageDatabaseIndex].FloorObject;
        Obstacles = StageDat.ItemList[StageDatabaseIndex].ObstaclesObject;
        Items = StageDat.ItemList[StageDatabaseIndex].ItemObject;
        StageDatabaseIndex++;
    }

    // Update is called once per frame
    void Update()
    {
    

    }
    void StageChangeAnoDifficultyUP() {
        if (XCoordinate % FrequencyOfStageChanges != 0) return;
        StageChange();
        DifficultyUP();
    }

    // このメソッドは、自動的に床とアイテムを生成し、X座標を更新します
    public void GenerationOfStages() {
        AutomaticFloor();
        AutomaticItems();
        XCoordinate++;
        StageChangeAnoDifficultyUP();
    }
    // このメソッドは、新しいゲームオブジェクトを指定された位置に生成し、floorObject配列に追加します
    void summonObject( GameObject Object, Vector2 pox) { 
       int ArrayLength = floorObject.Length;//配列の現在の長さを取得
        GameObject ball = Instantiate(Object, new Vector3(pox.y, pox.x, XCoordinate * 10), Quaternion.identity);
        ball.name = Object.name;
        Array.Resize(ref floorObject, ArrayLength+1);
        floorObject[ArrayLength] = ball;
        ball.transform.parent = this.transform;
    }
    // このメソッドは、指定されたゲームオブジェクトを再利用可能なオブジェクトプールから取得し、指定された位置に移動します
    void movingObject(GameObject Object, Vector2 pox)
    {
        foreach (GameObject item in floorObject)
        {
            if (item == null) continue;
            if (item.activeSelf != false) continue;
            if (item.tag != Object.tag) continue;
            if (item.name != Object.name) continue;
            item.SetActive(true);
            item.transform.position = new Vector3(pox.y, pox.x, XCoordinate * 10);
            return;
        }
        summonObject(Object,pox);

    }

    // このメソッドは、自動的に床を生成します
    void AutomaticFloor() {
        movingObject(floor, new Vector2(0, 0));
      
    }

    // このメソッドは、自動的にアイテムまたは障害物を生成します
    void AutomaticItems() {
        float Y = ItemWidth * (int)UnityEngine.Random.Range(-2f, 2f);
        if (!Probability(ObjectAppearanceProbability)) return;
        if (Probability(FrequencyOfObstacles))
        {
            ObstaclesGeneration(Y);
            return;
        }
        AutomaticPowerUpItemsGeneration(Y);
            return;  
    }
    void ObstaclesGeneration(float Y) {
        // 難易度に応じて障害物の生成方法を変更
        if (Probability(ProbabilityOf2Obstacles)) {
            AutomaticObstacles(Y);
            return;
        }
        AutomaticObstaclesGeneration(Y);
    }
    void AutomaticObstacles(float Y)
    {
        float A = Y / ItemWidth;
        switch (A) {
            case 1:
                AutomaticObstaclesGeneration(0* ItemWidth);
                AutomaticObstaclesGeneration(-1 * ItemWidth);
                break;
            case 0:
                AutomaticObstaclesGeneration(1 * ItemWidth);
                AutomaticObstaclesGeneration(-1 * ItemWidth);
                break;
            case -1:
                AutomaticObstaclesGeneration(1 * ItemWidth);
                AutomaticObstaclesGeneration(0 * ItemWidth);
                break;

        }
    }

    // このメソッドは、自動的に障害物を生成します
    void AutomaticObstaclesGeneration(float Y) {
        movingObject(Obstacles, new Vector2(1,Y));
    }
    // このメソッドは、自動的にパワーアップアイテムを生成します
    void AutomaticPowerUpItemsGeneration(float Y)
    {
        movingObject(Items, new Vector2(1, Y));
    }
    public static bool Probability(float fPercent)
    {
        float fProbabilityRate = UnityEngine.Random.value * 100.0f;

        if (fPercent == 100.0f && fProbabilityRate == fPercent)
        {
            return true;
        }
        else if (fProbabilityRate < fPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
