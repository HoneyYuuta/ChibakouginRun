using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField][Header("横移動速度")] private float sideSpeed;
    [SerializeField][Header("前移動速度")] private float frontSpeed;
    [SerializeField][Header("移動制限")] private float movementRestrictions;

    //左右の移動入力を保持する変数 (-1:左, 1:右, 0:停止)。GetAxisの値もここに入る（-1..1）
    private float horizontalInput = 0f;

    //UIボタンによる入力が有効かどうか
    private bool uiControlActive = false;

    //frontSpeed の public プロパティ
    public float FrontSpeed
    {
        get { return frontSpeed; }
        set { frontSpeed = value; }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        KeyboardController();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }



    private void KeyboardController()
    {
        
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
        //常に前進する速度
        Vector3 forwardVel = transform.forward * frontSpeed;

        //横移動（horizontalInputの値に比例）
        Vector3 horizontalVel = transform.right * sideSpeed * horizontalInput;

        //最終的な目標速度
        return forwardVel + horizontalVel;
    }

    //移動制限を考慮した速度を計算する
    private Vector3 ApplyMovementRestrictions(Vector3 velocity)
    {
        //次フレームの予測位置を計算し、x座標を制限内に収める
        float dt = Time.fixedDeltaTime;
        Vector3 projectedPos = rb.position + velocity * dt;
        float clampedX = Mathf.Clamp(projectedPos.x, -movementRestrictions, movementRestrictions);

        //予測位置が制限を超えていた場合、x方向の速度を補正する
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
        // Y成分は既存の物理挙動（重力など）を維持する
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }


    // --- UIボタンから呼び出すための公開メソッド ---

    //左移動を開始する
    public void StartMovingLeft()
    {
        horizontalInput = -1f;
        uiControlActive = true;
    }

    //右移動を開始する
    public void StartMovingRight()
    {
        horizontalInput = 1f;
        uiControlActive = true;
    }

    //横移動を停止する
    public void StopMoving()
    {
        horizontalInput = 0f;
        uiControlActive = false;
    }
}