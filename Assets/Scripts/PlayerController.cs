using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float sideSpeed;
    private float frontSpeed;
    private float movementRestrictions;

    // 左右の移動入力を保持する変数 (-1:左, 1:右, 0:停止)
    private float horizontalInput = 0f;

    // frontSpeed の public プロパティを追加
    public float flontSpeed
    {
        get { return frontSpeed; }
        set { frontSpeed = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        // 常に前進する速度
        Vector3 forwardVel = transform.forward * frontSpeed;

        // 横移動（左右入力）
        Vector3 horizontalVel = Vector3.zero;

        // Input.GetKeyの代わりに、horizontalInput変数の値で判断する
        if (horizontalInput > 0) // 右入力がある場合
        {
            horizontalVel = transform.right * sideSpeed;
        }
        else if (horizontalInput < 0) // 左入力がある場合
        {
            horizontalVel = -transform.right * sideSpeed;
        }

        // 予測される速度（前進 + 横移動）
        Vector3 desiredVel = forwardVel + horizontalVel;

        // 次フレームの予測位置を計算し、xをクランプする
        float dt = Time.fixedDeltaTime;
        Vector3 projectedPos = rb.position + desiredVel * dt;
        float clampedX = Mathf.Clamp(projectedPos.x, -movementRestrictions, movementRestrictions);

        if (!Mathf.Approximately(clampedX, projectedPos.x))
        {
            // 横方向の速度成分を補正して境界を越えないようにする（ワールドXで制限）
            float correctedXVel = (clampedX - rb.position.x) / dt;
            desiredVel.x = correctedXVel;
        }

        // Y成分は既存の物理挙動（重力など）を保持
        desiredVel.y = rb.velocity.y;

        rb.velocity = desiredVel;
    }

    // --- UIボタンから呼び出すための公開メソッド ---

    //左移動を開始する
    public void StartMovingLeft()
    {
        horizontalInput = -1f;
    }

    //右移動を開始する
    public void StartMovingRight()
    {
        horizontalInput = 1f;
    }

    //横移動を停止する
    public void StopMoving()
    {
        horizontalInput = 0f;
    }
}