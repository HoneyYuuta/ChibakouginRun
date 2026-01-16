using UnityEngine;

//プレイヤーの入力検知と各コンポーネントの統括を行うメインクラス
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerLaneMover))]
[RequireComponent(typeof(PlayerSpeedHandler))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerLaneMover laneMover;
    private PlayerSpeedHandler speedHandler;

    // ボタンを押したときtrue、離したときfalseになるフラグ
    private bool RightDownFlag = false;
    private bool LeftDownFlag = false;

    //長押し処理のアレコレ
    private float timeReset = 0f;
    private float time = 0f;

    //外部(UI等)からアクセスするためのプロパティ委譲
    public float CurrentSpeed => speedHandler.CurrentSpeed;
    public float BuffTimeRatio => speedHandler.BuffTimeRatio;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        laneMover = GetComponent<PlayerLaneMover>();
        speedHandler = GetComponent<PlayerSpeedHandler>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > timeReset)
        {
            //入力処理のみを担当し、実際の移動はMoverに依頼する
            if (Input.GetKey(KeyCode.A)|| LeftDownFlag) laneMover.MoveLeft();
        else if (Input.GetKey(KeyCode.D) || RightDownFlag) laneMover.MoveRight();
            time = 0;
        }
    }

    private void FixedUpdate()
    {
        //速度計算を依頼
        speedHandler.UpdateSpeed(Time.fixedDeltaTime);

        //計算された速度をRigidbodyに適用
        ApplyVelocity();
    }

    private void ApplyVelocity(Vector3 velocity = default) // 引数は拡張用
    {
        //前進速度はSpeedHandlerから取得
        Vector3 forwardVel = transform.forward * speedHandler.CurrentSpeed;

        //Y成分(重力)は維持して適用
        forwardVel.y = rb.velocity.y;
        rb.velocity = forwardVel;
    }

    //衝突判定
    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Items>();
        if (item != null)
        {
            item.ApplyEffect(this.gameObject);
        }
    }

    //外部(アイテム等)から呼ばれるメソッドの委譲

    public void ApplyTemporarySpeedUp(float duration)
    {
        speedHandler.ApplyTemporarySpeedUp(duration);
    }

    public void IncreaseLevel()
    {
        speedHandler.IncreaseLevel();
    }

    public void DecreaseLevel()
    {
        speedHandler.DecreaseLevel();
    }

    //アイテムから呼ばれる窓口
    public void ApplyTemporarySpeedDown(float duration)
    {
        speedHandler.ApplyTemporarySpeedDown(duration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ChibaCorgi"))
        {
            if (GameManager.Instance != null) GameManager.Instance.GameOver();
        }
    }

    public void OnRightButtonDown()
    {
        RightDownFlag = true;
    }
    public void OnRightButtonUp()
    {
        RightDownFlag = false;
    }
    public void OnLeftButtonDown()
    {
        LeftDownFlag = true;
    }
    public void OnLeftButtonUp()
    {
        LeftDownFlag = false;
    }

}