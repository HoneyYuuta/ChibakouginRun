using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
// 自動でフロアを生成するクラス
public class AutomaticFloor : MonoBehaviour
{
    [SerializeField] Transform playerTransform;// プレイヤーのTransform
    public AutoBackground autoBackground;
    [SerializeField] ItemDataBase stageDB;// ステージ生成に使うデータベース
    [SerializeField] ObjectProbabilityDatabase probabilityDB;// ステージ生成に使うデータベース
    [SerializeField] StageChangeScript stageChangeScript;//ステージ変更スクリプト
    [SerializeField] GameObject[] poolObjects;// プールするオブジェクト
    [SerializeField] float floorWidth = 15f;// 各フロアセグメントの長さ
    [SerializeField] float itemWidth = 1.5f;// 各アイテムの幅

   
    float NextStageChangeCoordinate = 0f;// 次のステージ変更座標

    StageController stageController;// ステージ管理クラス
    ObjectPooler pooler;// オブジェクトプーラー
    StageGenerator generator;// ステージ生成クラス
    SegmentLottery lottery;// セグメント抽選クラス

    int x;

    void Start()
    {
        pooler = new ObjectPooler(poolObjects, transform);
        stageController = new StageController(stageDB, probabilityDB);
        generator = new StageGenerator(pooler, stageController, floorWidth, itemWidth);
      
        lottery = new SegmentLottery();
        // 最初の数フロアを生成
        for (int i = 0; i < 10; i++)
        {
            generator.GenerateFloor(x);
           
            x++;
        }

    }

    public void Generate()
    {
        generator.GenerateConnection(x);
        autoBackground.AutomaticBackground(x , floorWidth);
        x++;
    }
    void Update()
    {
        if (playerTransform == null) return;
        if (playerTransform.position.z <= NextStageChangeCoordinate) return;
        stageChangeScript.StageUpdate(generator.GetCurrentStageName());// ステージ変更を通知
        NextStageChangeCoordinate += generator.UpdateNextStageChangeCoordinate();// 次のステージ変更座標を更新
    }

}
