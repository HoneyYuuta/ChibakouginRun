using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Items;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField][Header("横移動速度")] private float sideSpeed;
    [SerializeField][Header("移動制限")] private float movementRestrictions;

    //インスペクターからSpeedDatabaseを設定するための変数
    [Header("データベース")]
    [SerializeField] private speedDatabase speedDatabase;

    //現在のプレイヤーの速度レベルを管理する変数
    private int currentLevel = 0;

    //左右の移動入力を保持する変数 (-1:左, 1:右, 0:停止)。
    private float horizontalInput = 0f;

    //UIボタンによる入力が有効かどうか
    private bool uiControlActive = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //ゲーム開始時にデータベースが設定されているか確認
        if (speedDatabase == null)
        {
            Debug.LogError("PlayerControllerにSpeedDatabaseが設定されていません！");
        }
    }

    void Update()
    {
        //KeyboardController();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }


    //アイテムとの衝突を検知するためのメソッド
    private void OnTriggerEnter(Collider other)
    {
        //衝突した相手が「IItem」というルール(インターフェース)を持っているかチェック
        Items item = other.GetComponent<Items>();
        if (item != null)
        {
            //持っていたら、そのアイテムのApplyEffectメソッドを呼び出す
            //引数には自分自身(プレイヤー)のGameObjectを渡す
            item.ApplyEffect(this.gameObject);
        }
    }


    //移動処理の本体
    private void HandleMovement()
    {
        //1. 目標となる速度を計算する
        Vector3 desiredVel = CalculateDesiredVelocity();

        //2. 移動範囲の制限を適用する
        Vector3 restrictedVel = ApplyMovementRestrictions(desiredVel);

        //3. Rigidbodyの速度を更新する
        ApplyVelocity(restrictedVel);
    }

    //入力に基づいた目標速度を計算する
    private Vector3 CalculateDesiredVelocity()
    {
        //frontSpeedを直接使う代わりに、データベースから現在のレベルに応じた速度を取得する
        float currentFrontSpeed = speedDatabase.GetSpeedForLevel(currentLevel);
        Vector3 forwardVel = transform.forward * currentFrontSpeed;

        //横移動（horizontalInputの値に比例）
        Vector3 horizontalVel = transform.right * sideSpeed * horizontalInput;

        //最終的な目標速度
        return forwardVel + horizontalVel;
    }

    //移動制限を考慮した速度を計算する
    private Vector3 ApplyMovementRestrictions(Vector3 velocity)
    {
        float dt = Time.fixedDeltaTime;
        Vector3 projectedPos = rb.position + velocity * dt;
        float clampedX = Mathf.Clamp(projectedPos.x, -movementRestrictions, movementRestrictions);

        if (!Mathf.Approximately(clampedX, projectedPos.x))
        {
            float correctedXVel = (clampedX - rb.position.x) / dt;
            velocity.x = correctedXVel;
        }
        return velocity;
    }

    //計算された速度をRigidbodyに適用する
    private void ApplyVelocity(Vector3 velocity)
    {
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }


    // --- アイテム側から呼ばれる、レベルを操作するための公開メソッド ---

    /// プレイヤーの速度レベルを1上げる
    public void IncreaseLevel()
    {
        // 最大レベルを超えないようにチェック
        if (currentLevel < speedDatabase.GetMaxLevel())
        {
            currentLevel++;
            Debug.Log("Level Up! 新しいレベル: " + currentLevel + ", 新しい速度: " + speedDatabase.GetSpeedForLevel(currentLevel));
        }
    }

    //プレイヤーの速度レベルを1下げる
    public void DecreaseLevel()
    {
        //レベル0より下に行かないようにチェック
        if (currentLevel > 0)
        {
            currentLevel--;
            Debug.Log("Level Down! 新しいレベル: " + currentLevel + ", 新しい速度: " + speedDatabase.GetSpeedForLevel(currentLevel));
        }
    }


    // --- UIボタンから呼び出すための公開メソッド ---
    public void StartMovingLeft()
    {
        horizontalInput = -1f;
        uiControlActive = true;
    }
    public void StartMovingRight()
    {
        horizontalInput = 1f;
        uiControlActive = true;
    }
    public void StopMoving()
    {
        horizontalInput = 0f;
        uiControlActive = false;
    }
}