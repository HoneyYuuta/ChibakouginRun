using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEditor.Progress;

public class automaticFloor : MonoBehaviour
{
    public GameObject floor;//床のオブジェクト
    public GameObject Obstacles;//障害物のオブジェクト
    public GameObject Items;//アイテムのオブジェクト
    [SerializeField]
    private ItemDataBase StageDat;//ステージデータベース
    public GameObject[] floorObject;//床オブジェクトの配列
    public int XCoordinate=0;//X座標
    public float ItemWidth = 1.5f;//アイテムの幅
    int StageDatabaseIndex = 0;//ステージデータベースのインデックス
    public int FrequencyOfStageChanges = 100;//ステージ変更の頻度
    // Start is called before the first frame update
    void Start()
    {
        StageChange();
        for (int i = 0; i < 10; i++)
        {
            movingObject(floor, new Vector2(0, 0));
            XCoordinate++;
        }
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
        if (XCoordinate %  FrequencyOfStageChanges != 0) return;
        StageChange();

    }

    // このメソッドは、自動的に床とアイテムを生成し、X座標を更新します
    public void GenerationOfStages() {
        AutomaticFloor();
        AutomaticItems();
        XCoordinate++;
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
        int number = (int)UnityEngine.Random.Range(1f,11f);
        Debug.Log(number);
        if (number > 7) return;
        float Y = ItemWidth * (int)UnityEngine.Random.Range(-2f, 2f);
      
        if (number <= 4)
        {
            AutomaticObstacles(Y);
            return;
        }
        AutomaticPowerUpItems(Y);
    }

    // このメソッドは、自動的に障害物を生成します
    void AutomaticObstacles(float Y) {
        movingObject(Obstacles, new Vector2(1,Y));
    }
    // このメソッドは、自動的にパワーアップアイテムを生成します
    void AutomaticPowerUpItems(float Y)
    {
        movingObject(Items, new Vector2(1, Y));
    }
}
