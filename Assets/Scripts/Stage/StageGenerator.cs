using UnityEngine;

public class StageGenerator
{
    ObjectPooler pooler;
    private float floorWidth;//床の幅
    private float itemWidth;//アイテムの幅
    SegmentLottery lottery;
    StageController stageController;
    int XCoordinate = 0;//X座標

    public SegmentType lastSegment = SegmentType.Safe;
    public const int FrequencyOfStageChanges = 100;//ステージ変更の頻度

    public StageGenerator(ObjectPooler pool,StageController stage, float floorW, float itemW)
    {
        pooler = pool;
        stageController = stage;
        floorWidth = floorW;
        itemWidth = itemW;
        lottery = new SegmentLottery();
        stageController.NextStage();
    }
    Lane RandomLane()
    {
        return (Lane)UnityEngine.Random.Range(-1, 2);
    }

    float LaneToY(Lane lane)
    { return (int)lane * itemWidth; }

    // フロアを生成するメソッド
    public void GenerateConnection(int x)
    {
        GenerateFloor(x);
        AutomaticItems(x);
        if(x % FrequencyOfStageChanges == 0)
        {
            stageController.NextStage();
        }
    }

    public void GenerateFloor(int x)
    {
        pooler.Spawn(stageController.Floor, new Vector3(0, 0, x * floorWidth));
    }
    // 障害物を生成するメソッド
    public void GenerateObstacle(int x, float y)
    {
        pooler.Spawn(stageController.Obstacles, new Vector3(y, 1, x * floorWidth));
    }
    // アイテムを生成するメソッド
    public void GenerateItem(int x, float y)
    {
        pooler.Spawn(stageController.Items, new Vector3(y, 1, x * floorWidth));
    }
    void movingObject(GameObject obj,Vector2 vec)
    {
        pooler.Spawn(obj, new Vector3(vec.y, vec.x, XCoordinate * floorWidth));
    }
    void GenerateRecovery(int x, float y)
    {
        pooler.Spawn(stageController.Recovery, new Vector3(y, 1, x * floorWidth));
    }
    SegmentType DecideSegment()
    {
        // 休憩を定期的に入れる
        if (floorWidth % FrequencyOfStageChanges == 0)
            return SegmentType.Safe;
        SegmentType type = lottery.Draw(stageController.items);
        if (lastSegment != SegmentType.Double) return type;
        if (type != SegmentType.Double) return type;
        return SegmentType.Single;
    }
    // このメソッドは、自動的にアイテムまたは障害物を生成します
    void AutomaticItems(int x)
    {
        XCoordinate = x;
        lastSegment = DecideSegment();
        GenerateObstacles();
    }
    // このメソッドは、障害物を生成します
    void GenerateObstacles()
    {
        float Y = LaneToY(RandomLane());
        switch (lastSegment)
        {
            case SegmentType.Safe://安全
                return;

            case SegmentType.Single://シングル
                GenerateObstacle(XCoordinate, Y);
                break;

            case SegmentType.Double://ダブル
                AutomaticObstacles(Y);
                break;

            case SegmentType.Shift://シフト
                GenerateItem(XCoordinate, Y);
                break;

            case SegmentType.Recovery://回復
                GenerateRecovery(XCoordinate, Y);
                break;
        }
    }
    void AutomaticObstacles(float Y)
    {
        float A = Y / itemWidth;

        switch (A)
        {
            case 1:
                movingObject(stageController.Connection, new Vector2(1, 0));
              
                break;
            case 0:
                GenerateObstacle(XCoordinate, 1 * itemWidth);
                GenerateObstacle(XCoordinate, -1 * itemWidth);
                break;
            case -1:
                movingObject(stageController.Connection, new Vector2(1, 1 * itemWidth));
                break;

        }
    }
    // 現在のステージ名
    public string GetCurrentStageName()
    {
        return stageController.StageName;
    }
    // 次のステージ変更座標を更新
    public float UpdateNextStageChangeCoordinate()
    {
        return (FrequencyOfStageChanges * floorWidth);
    }
}