using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
// using static Items; // Itemsクラスの定義場所によりますが、エラーが出る場合は外してください

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("データベース")]
    [SerializeField] private speedDatabase speedDatabase; // クラス名が大文字始まり(SpeedDatabase)か確認してください
    [SerializeField][Header("横移動にかける時間")] private float lateralMoveDuration;
    [SerializeField][Header("横移動の距離（レーン幅）")] private float lateralMoveDistance;

    private int currentLevel = 0;

    // ▼追加：現在のレーン位置を管理 (-1:左, 0:中央, 1:右)
    private int currentLaneIndex = 0;

    // ▼追加：移動中かどうかを判定するフラグ（連打防止用）
    private bool isLaneChanging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (speedDatabase == null)
        {
            Debug.LogError("PlayerControllerにSpeedDatabaseが設定されていません！");
        }
    }

    void Update()
    {
        // 横移動は Dotween に任せるため処理なし
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Itemsクラスの定義に合わせて適宜修正してください
        var item = other.GetComponent<Items>(); // 型名をItemsに合わせてください
        if (item != null)
        {
            item.ApplyEffect(this.gameObject);
        }
    }

    private void HandleMovement()
    {
        Vector3 desiredVel = CalculateDesiredVelocity();
        ApplyVelocity(desiredVel);
    }

    private Vector3 CalculateDesiredVelocity()
    {
        float currentFrontSpeed = speedDatabase != null ? speedDatabase.GetSpeedForLevel(currentLevel) : 0f;
        Vector3 forwardVel = transform.forward * currentFrontSpeed;
        return forwardVel;
    }

    private void ApplyVelocity(Vector3 velocity)
    {
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    public void IncreaseLevel()
    {
        if (speedDatabase == null) return;
        if (currentLevel < speedDatabase.GetMaxLevel())
        {
            currentLevel++;
            Debug.Log("Level Up! " + currentLevel);
        }
    }

    public void DecreaseLevel()
    {
        if (currentLevel > 0)
        {
            currentLevel--;
            Debug.Log("Level Down! " + currentLevel);
        }
    }

    // --- ▼▼▼ ここから横移動の修正部分 ▼▼▼ ---

    public void StartMovingLeft()
    {
        // 移動中なら何もしない（連打防止）
        if (isLaneChanging) return;

        // 現在のレーンが「左端(-1)」より大きい場合のみ移動可能
        if (currentLaneIndex > -1)
        {
            currentLaneIndex--; // レーン番号を1つ減らす
            MoveToLane();
        }
    }

    public void StartMovingRight()
    {
        // 移動中なら何もしない
        if (isLaneChanging) return;

        // 現在のレーンが「右端(1)」より小さい場合のみ移動可能
        if (currentLaneIndex < 1)
        {
            currentLaneIndex++; // レーン番号を1つ増やす
            MoveToLane();
        }
    }

    // 実際の移動処理をまとめたメソッド
    private void MoveToLane()
    {
        isLaneChanging = true; // 移動開始フラグを立てる

        // 目標のX座標を計算 (レーン番号 × レーン幅)
        // 例：幅が2の場合 -> 左(-2), 中央(0), 右(2) となる
        float targetX = currentLaneIndex * lateralMoveDistance;

        // DOTweenで移動
        transform.DOLocalMoveX(targetX, lateralMoveDuration)
            .SetEase(Ease.OutQuad)
            .SetLink(gameObject) // オブジェクト破棄時の安全策
            .OnComplete(() =>
            {
                // 移動が終わったらフラグを下ろす
                isLaneChanging = false;
            });
    }

    public void StopMoving()
    {
        // DOTweenの場合は自動で止まるので、ここは空でOK
    }
}