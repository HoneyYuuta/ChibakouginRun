using System;
using UnityEngine;


public class automaticFloor : MonoBehaviour
{
    GameObject floor;//床のオブジェクト
    GameObject Obstacles;//障害物のオブジェクト
    GameObject Items;//アイテムのオブジェクト
    GameObject Connection;//接続オブジェクト
    string stageName;
    [SerializeField]
    Transform playerTransform;//プレイヤーのトランスフォーム
    [SerializeField]
    private ItemDataBase StageDat;//ステージデータベース
    [SerializeField]
    private ObjectProbabilityDatabase objectProbability;//オブジェクト確率データ
    int DifficultyLevel = 0;//難易度レベル
    //プレイヤーが一定の 座標に到達したときに数値
    float NextStageChangeCoordinate = 0;
    [SerializeField] GameObject[] floorObject;//床オブジェクトの配列
    public int XCoordinate=0;//X座標
    [SerializeField] float ItemWidth = 1.5f;//アイテムの幅                       
    [SerializeField] float FloorWidth = 15f;//床の幅
    int StageDatabaseIndex = 0;//ステージデータベースのインデックス
    [SerializeField] int FrequencyOfStageChanges = 100;//ステージ変更の頻度
    int FrequencyOfObstacles = 40;//障害物の確率
    int ObjectAppearanceProbability = 30;//アイテムの確率
    int ProbabilityOf2Obstacles = 0;//2つの障害物の確率
    [SerializeField]
    public StageChangeScript stageChangeScript;

    enum Lane { Left = -1, Center = 0, Right = 1 }

    Lane RandomLane()
    {
        return (Lane)UnityEngine.Random.Range(-1, 2);
    }
    float LaneToY(Lane lane)
    {
    return (int)lane * ItemWidth;
    }
    // 重み付き抽選のセグメントタイプを定義
    public enum SegmentType
    {
        Safe,//安全
        Single,//シングル
        Double,//ダブル
        Shift//シフト
    }

    public class WeightedItem
    {
        public SegmentType segmentType;
        public int weight;
    }
    SegmentType WeightedLottery(WeightedItem[] items)
    {
        int totalWeight = 0;
        foreach (var item in items)
            totalWeight += item.weight;

        int r = UnityEngine.Random.Range(0, totalWeight);
        int current = 0;

        foreach (var item in items)
        {
            current += item.weight;
            if (r < current)
                return item.segmentType;
        }

        throw new System.Exception("抽選失敗");
    }
    WeightedItem[] items = new WeightedItem[4];
    SegmentType lastSegment;
    public void Add(){
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
    // Start is called before the first frame update
    void Start()
    {
        Add();
        StageChange();
        DifficultyUP();
        for (int i = 0; i < StageDat.ItemList.Count; i++)
        {

            for (int j = 0; j < 10; j++)
            {
                InitialItems(Items);
                InitialItems(Obstacles);
                InitialItems(Connection);
                StageChange();

            }
        }

        for (int i = 0; i < 10; i++)
        {
            movingObject(floor, new Vector2(0, 0));
            XCoordinate++;
        }
      
    }
    
    void DifficultyUP() { 
    if(objectProbability == null) return;
    if(objectProbability.ItemList.Count <= DifficultyLevel) return;
        items[0].weight = objectProbability.ItemList[DifficultyLevel].SkyProbability;
        items[1].weight = objectProbability.ItemList[DifficultyLevel].ObstacleProbability;
        items[2].weight = objectProbability.ItemList[DifficultyLevel].ItemProbability;
        items[3].weight = objectProbability.ItemList[DifficultyLevel].ProbabilityOf2Obstacles;
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
        Connection = StageDat.ItemList[StageDatabaseIndex].ConnectionObject;
        stageName = StageDat.ItemList[StageDatabaseIndex].stageName;
        StageDatabaseIndex++;
    }

    // Update is called once per frame
    void Update()
    {
     if (playerTransform == null) return;
     if (playerTransform.position.z <= NextStageChangeCoordinate) return;
        stageChangeScript.StageUpdate(stageName);
        NextStageChangeCoordinate += (FrequencyOfStageChanges * FloorWidth);
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
        GameObject ball = Instantiate(Object, new Vector3(pox.y, pox.x, XCoordinate * FloorWidth), Quaternion.identity);
        ball.name = Object.name;
        Array.Resize(ref floorObject, ArrayLength+1);
        floorObject[ArrayLength] = ball;
        ball.transform.parent = this.transform;
    }
    //初期アイテム
    void InitialItems(GameObject Object) {
        int ArrayLength = floorObject.Length;//配列の現在の長さを取得
        GameObject ball = Instantiate(Object, new Vector3(0, -10, 0), Quaternion.identity);
        ball.SetActive(false);
        ball.name = Object.name;
        Array.Resize(ref floorObject, ArrayLength + 1);
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
            item.transform.position = new Vector3(pox.y, pox.x, XCoordinate * FloorWidth);
            return;
        }
        summonObject(Object,pox);

    }


    // このメソッドは、自動的に床を生成します
    void AutomaticFloor() {
        movingObject(floor, new Vector2(0, 0));
    }

    SegmentType DecideSegment()
    {
        // 休憩を定期的に入れる
        if (XCoordinate % FrequencyOfStageChanges == 0) 
        return SegmentType.Safe;
        SegmentType type = WeightedLottery(items);
        if (lastSegment != SegmentType.Double) return type;
        if (type != SegmentType.Double) return type;
        return SegmentType.Single;
    }
    // このメソッドは、自動的にアイテムまたは障害物を生成します
    void AutomaticItems() {
        lastSegment = DecideSegment();
        GenerateObstacles();
    }
    void GenerateObstacles()
    {
        float Y = LaneToY(RandomLane());
        switch (lastSegment)
        {
            case SegmentType.Safe://安全
                return;

            case SegmentType.Single://シングル
                AutomaticObstaclesGeneration(Y);
                break;

            case SegmentType.Double://ダブル
                AutomaticObstacles(Y);
                break;

            case SegmentType.Shift://シフト
                AutomaticPowerUpItemsGeneration(Y);
                break;
        }
    }
    void AutomaticObstacles(float Y)
    {
        float A = Y / ItemWidth;

        switch (A) {
            case 1:
                movingObject(Connection, new Vector2(1, 0));
                AutomaticObstaclesGeneration(0* ItemWidth);
                AutomaticObstaclesGeneration(-1 * ItemWidth);
                break;
            case 0:
                AutomaticObstaclesGeneration(1 * ItemWidth);
                AutomaticObstaclesGeneration(-1 * ItemWidth);
                break;
            case -1:
                movingObject(Connection, new Vector2(1, 1 * ItemWidth));
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
}
