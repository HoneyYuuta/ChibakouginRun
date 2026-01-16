using System.Collections;
using UnityEngine;

//プレイヤーの速度計算、レベル管理、バフタイマーを担当するコンポーネント
public class PlayerSpeedHandler : MonoBehaviour
{
    [SerializeField][Header("データベース")] private speedDatabase speedDatabase;
    [SerializeField][Header("加速度設定")] private float accelerationSpeed = 2.0f;

    private int currentLevel = 0;
    private float currentActualSpeed = 0f;
    private bool isStopping = false;

    //バフ関連
    private Coroutine speedResetCoroutine;
    private float currentBuffDuration;
    public float BuffTimeRatio { get; private set; } = 0f;

    //減速からの回復用タイマー
    private Coroutine speedDownTime;

    //外部公開用の現在の速度プロパティ
    public float CurrentSpeed => currentActualSpeed;

    private void Start()
    {
        if (speedDatabase == null) Debug.LogError("SpeedDatabaseが設定されていません！");
        else currentActualSpeed = speedDatabase.GetSpeedForLevel(0);
    }

    //毎フレームの速度計算（FixedUpdateで呼ばれることを想定）
    public void UpdateSpeed(float deltaTime)
    {
        float targetSpeed = isStopping ? 0f : speedDatabase.GetSpeedForLevel(currentLevel);

        //滑らかに加速・減速
        currentActualSpeed = Mathf.Lerp(currentActualSpeed, targetSpeed, deltaTime * accelerationSpeed);

        //停止判定（ゲームオーバーチェック）
        CheckGameOverCondition();
    }

    //ゲームオーバー条件のチェック
    private void CheckGameOverCondition()
    {
        if (isStopping && currentActualSpeed < 0.1f)
        {
            currentActualSpeed = 0f;
            Debug.Log("完全に停止しました。ゲームオーバー！");
            if (GameManager.Instance != null) GameManager.Instance.GameOver();
        }
    }

    //外部から操作されるメソッド群

    public void ApplyTemporarySpeedUp(float duration)
    {
        if (isStopping) isStopping = false; //復帰処理
        currentBuffDuration = duration;
        IncreaseLevel();
    }

    public void IncreaseLevel()
    {
        if (currentLevel < speedDatabase.GetMaxLevel())
        {
            currentLevel++;
            Debug.Log($"Level Up! {currentLevel}");
        }
        ResetDecayTimer();
    }

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

    //一時的な減速処理
    public void ApplyTemporarySpeedDown(float duration)
    {
        // すでに減速からの回復待ち（タイマーが動いている）かどうかチェック
        if (speedDownTime != null)
        {
            StopCoroutine(speedDownTime);
            Debug.Log("減速効果延長（レベルは維持）");
        }
        else
        {
            DecreaseLevel();
        }

        // 新しい回復タイマーをスタート（延長または新規開始）
        speedDownTime = StartCoroutine(RecoveryRoutine(duration));
    }

    //指定時間待ってからレベルを戻すコルーチン
    private IEnumerator RecoveryRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        //時間が経過したらレベルを戻す
        IncreaseLevel();

        //タイマー変数を空にする
        speedDownTime = null;
    }

    //タイマー処理

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
}