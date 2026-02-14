using System.Collections;
using UnityEngine;

//プレイヤーの速度計算、レベル管理、バフタイマーを担当するコンポーネント

public class PlayerSpeedHandler : MonoBehaviour
{
    [SerializeField][Header("データベース")] private speedDatabase speedDatabase;
    [SerializeField][Header("加速度設定")] private float accelerationSpeed = 3.0f;
    [SerializeField][Header("減速度設定")] private float decelerationSpeed = 10.0f;

    //自動レベルアップの設定
    [Header("自動レベルアップ設定")]
    [SerializeField] private bool enableAutoLevelUp = true; //有効/無効
    [SerializeField] private float autoLevelUpInterval = 10.0f; //何秒ごとに上げるか

    private int currentLevel = 0;
    private float currentActualSpeed = 0f;
    private bool isStopping = false;

    //速度倍率（通常は1.0 = 100%）
    private float speedMultiplier = 1.0f;

    //自動レベルアップ用タイマー
    private float autoLevelTimer = 0f;

    //バフ・デバフタイマー関連
    private Coroutine speedRecoveryCoroutine; //減速からの回復用
    private Coroutine speedResetCoroutine;    //加速アイテム用
    private float currentBuffDuration;
    public float BuffTimeRatio { get; private set; } = 0f;

    //外部公開用の現在の速度プロパティ
    public float CurrentSpeed => currentActualSpeed;

    private void Start()
    {
        if (speedDatabase == null) Debug.LogError("SpeedDatabaseが設定されていません！");
        else currentActualSpeed = speedDatabase.GetSpeedForLevel(0);
    }

    //毎フレームの速度計算
    public void UpdateSpeed(float deltaTime)
    {
        //自動レベルアップのタイマー処理
        if (!isStopping && enableAutoLevelUp)
        {
            autoLevelTimer += deltaTime;
            if (autoLevelTimer >= autoLevelUpInterval)
            {
                autoLevelTimer = 0f; //タイマーリセット
                IncreaseLevel();     //レベルを上げる
                Debug.Log("時間経過でレベルアップしました");
            }
        }


        //データベースから基本速度を取得
        float baseSpeed = isStopping ? 0f : speedDatabase.GetSpeedForLevel(currentLevel);

        //基本速度に倍率を掛けて「目標速度」とする
        float targetSpeed = baseSpeed * speedMultiplier;

        //加速減速処理
        if (targetSpeed < currentActualSpeed)
        {
            //減速する時
            currentActualSpeed = Mathf.Lerp(currentActualSpeed, targetSpeed, deltaTime * decelerationSpeed);
        }
        else
        {
            //加速する時
            currentActualSpeed = Mathf.Lerp(currentActualSpeed, targetSpeed, deltaTime * accelerationSpeed);
        }

        //停止判定
        CheckGameOverCondition();
    }

    public void CheckGameOverCondition()
    {
        if (isStopping && currentActualSpeed < 0.1f)
        {
            currentActualSpeed = 0f;
            Debug.Log("完全に停止しました。ゲームオーバー！");
            GameManager.Instance.GameOver();
        }
    }

    //外部から操作されるメソッド群

    //加速アイテムの処理(一定時間速度アップ)
    public void ApplyTemporarySpeedUp(float duration)
    {
        if (isStopping) isStopping = false;
        currentBuffDuration = duration;
        IncreaseLevel();
    }

    //障害物にぶつかったら割合で速度を下げる処理
    public void ApplyPercentageSpeedDown(float duration, float penaltyRatio)
    {
        //プレイヤーの体力管理
        var lifePoints = FindObjectOfType<LifePointsScript>();
        if (lifePoints != null)
        {
            lifePoints.Damage();
            if (lifePoints.IsDead())
            {
                decelerationSpeed = 3.0f;
                StopMovement();
                return;
            }
        }

        //倍率を設定 (例: 1.0 - 0.1 = 0.9)
        speedMultiplier = 1.0f - penaltyRatio;
        Debug.Log($"速度 {penaltyRatio * 100}% ダウン！ 現在の倍率: {speedMultiplier}");

        //すでに減速中ならタイマーをリセット
        if (speedRecoveryCoroutine != null)
        {
            StopCoroutine(speedRecoveryCoroutine);
        }

        //回復タイマーを開始
        speedRecoveryCoroutine = StartCoroutine(RecoveryRoutine(duration));
    }

    //減速から回復するコルーチン
    private IEnumerator RecoveryRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        // 時間が経過したら倍率を1.0(100%)に戻す
        speedMultiplier = 1.0f;
        Debug.Log("速度制限解除！");

        speedRecoveryCoroutine = null;
    }

    //加速アイテムの処理(レベルが1上昇)
    public void IncreaseLevel()
    {
        if (currentLevel < speedDatabase.GetMaxLevel())
        {
            currentLevel++;
            Debug.Log($"Level Up! {currentLevel}");
        }
        ResetDecayTimer();
    }

    //減速アイテムの処理(レベルが1下降)
    public void DecreaseLevel()
    {
        if (currentLevel > 0)
        {
            currentLevel--;
            Debug.Log($"Level Down! {currentLevel}");
            ResetDecayTimer();
        }
        else
        {
            Debug.Log("これ以上下がれない！停止モードへ移行");
            isStopping = true;
            StopDecayTimer();
        }
    }

    private void ResetDecayTimer()
    {
        StopDecayTimer();
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

    private IEnumerator DecayRoutine()
    {
        float timer = currentBuffDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            BuffTimeRatio = timer / currentBuffDuration;
            yield return null;
        }
        BuffTimeRatio = 0f;
        DecreaseLevel();
    }

    public void StopMovement()
    {
        isStopping = true;
        StopDecayTimer();
    }
}