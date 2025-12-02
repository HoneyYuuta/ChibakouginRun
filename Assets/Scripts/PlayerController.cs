using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Coroutine speedResetCoroutine;
    private float currentBuffDuration;

    [SerializeField][Header("データベース")] private speedDatabase speedDatabase;
    [SerializeField][Header("横移動にかける時間")] private float lateralMoveDuration;
    [SerializeField][Header("横移動の距離（レーン幅）")] private float lateralMoveDistance;
    [SerializeField][Header("加速度設定")] private float accelerationSpeed = 2.0f;
    private int currentLevel = 0;

    //現在、実際に出ている速度（内部計算用）
    private float currentActualSpeed = 0f;

    //現在のレーン位置を管理 (-1:左, 0:中央, 1:右)
    private int currentLaneIndex = 0;

    //移動中かどうかを判定するフラグ（連打防止用）
    private bool isLaneChanging = false;

    //UI表示用に、現在の残り時間の割合(0.0〜1.0)を保持する変数
    public float BuffTimeRatio { get; private set; } = 0f;

    public float CurrentSpeed
    {
        get
        {
            // 滑らかに変化している最中の値を返す
            return currentActualSpeed;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (speedDatabase == null)
        {
            Debug.LogError("PlayerControllerにSpeedDatabaseが設定されていません！");
        }
        else
        {
            // ゲーム開始時は、レベル0の速度を初期値にする
            currentActualSpeed = speedDatabase.GetSpeedForLevel(0);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartMovingLeft();
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            StartMovingRight();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Itemsクラスの定義に合わせて適宜修正してください
        var item = other.GetComponent<Items>(); //型名をItemsに合わせてください
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
        //今のレベルにおける「目標とすべき速度」を取得
        float targetSpeed = speedDatabase != null ? speedDatabase.GetSpeedForLevel(currentLevel) : 0f;

        //「現在の実際の速度」を「目標速度」に向けて徐々に近づける
        currentActualSpeed = Mathf.Lerp(currentActualSpeed, targetSpeed, Time.fixedDeltaTime * accelerationSpeed);

        //レベルが0、かつ、実際の速度が 0.1 未満になったら
        if (currentLevel == 0 && currentActualSpeed < 0.1f)
        {
            //速度を完全に0にする
            currentActualSpeed = 0f;

            //ゲームオーバー処理を実行
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }

        //計算した「実際の速度」を使って進む
        Vector3 forwardVel = transform.forward * currentActualSpeed;
        return forwardVel;
    }

    private void ApplyVelocity(Vector3 velocity)
    {
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    public void ApplyTemporarySpeedUp(float duration)
    {
        //今回の効果時間を保存（上書き）
        currentBuffDuration = duration;

        //レベルを上げる（タイマーのリセットはIncreaseLevel内で行われる）
        IncreaseLevel();
    }

    //プレイヤーの速度レベルを1上げる
    public void IncreaseLevel()
    {
        if (currentLevel < speedDatabase.GetMaxLevel())
        {
            currentLevel++;
            Debug.Log("Level Up! " + currentLevel);
        }
        // レベルが変わったのでタイマーをリセットする
        ResetDecayTimer();
    }

    //プレイヤーの速度レベルを1下げる
    public void DecreaseLevel()
    {
        if (currentLevel > 0)
        {
            currentLevel--;
            Debug.Log("Level Down! " + currentLevel);

            //レベルが変わったのでタイマーをリセットする
            ResetDecayTimer();
        }
        else
        {
            //レベル0になったらタイマーは不要なので止める
            StopDecayTimer();
        }
    }

    //タイマー管理用のヘルパーメソッド

    private void ResetDecayTimer()
    {
        //既存のタイマーがあれば止める
        StopDecayTimer();

        //レベルが0より大きく、かつ効果時間が設定されていれば、新しいタイマーをスタート
        if (currentLevel > 0 && currentBuffDuration > 0)
        {
            speedResetCoroutine = StartCoroutine(DecayRoutine());
        }
    }

    private void StopDecayTimer()
    {
        if (speedResetCoroutine != null)
        {
            StopCoroutine(speedResetCoroutine);
            speedResetCoroutine = null;
        }
        BuffTimeRatio = 0f;
    }

    //時間が来たらレベルを下げるコルーチン
    private IEnumerator DecayRoutine()
    {
        float timer = currentBuffDuration;

        //タイマーが0になるまでループする
        while (timer > 0)
        {
            //1フレーム分の時間を引く
            timer -= Time.deltaTime;

            //UI用に「残り時間の割合」を計算 (1.0 → 0.0 へ減っていく)
            BuffTimeRatio = timer / currentBuffDuration;

            //次のフレームまで待つ
            yield return null;
        }

        //時間切れ処理
        BuffTimeRatio = 0f;
        DecreaseLevel();
    }

    public void StartMovingLeft()
    {
        //移動中なら何もしない（連打防止）
        if (isLaneChanging) return;

        //現在のレーンが「左端(-1)」より大きい場合のみ移動可能
        if (currentLaneIndex > -1)
        {
            currentLaneIndex--; //レーン番号を1つ減らす
            MoveToLane();
        }
    }

    public void StartMovingRight()
    {
        //移動中なら何もしない
        if (isLaneChanging) return;

        //現在のレーンが「右端(1)」より小さい場合のみ移動可能
        if (currentLaneIndex < 1)
        {
            currentLaneIndex++; //レーン番号を1つ増やす
            MoveToLane();
        }
    }

    //実際の移動処理をまとめたメソッド
    private void MoveToLane()
    {
        isLaneChanging = true;//移動開始フラグを立てる

        //目標のX座標を計算(レーン番号×レーン幅)
        //例：幅が2の場合->左(-2),中央(0),右(2)となる
        float targetX = currentLaneIndex * lateralMoveDistance;

        //DOTweenで移動
        transform.DOLocalMoveX(targetX, lateralMoveDuration)
            .SetEase(Ease.OutQuad)
            .SetLink(gameObject) // オブジェクト破棄時の安全策
            .OnComplete(() =>
            {
                // 移動が終わったらフラグを下ろす
                isLaneChanging = false;
            });
    }
}