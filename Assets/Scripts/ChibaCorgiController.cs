using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChibaCorgiController : MonoBehaviour
{
    Rigidbody rb;

    [Header("データベース")]
    [SerializeField] private speedDatabase speedDatabase;
    [SerializeField][Header("前移動速度（フォールバック）")] private float frontSpeed;

    [Header("自動レベルアップ設定")]
    [SerializeField][Tooltip("何秒ごとにレベルを上げるか")] private float levelUpInterval = 20f;
    [SerializeField][Tooltip("自動レベルアップを有効にするか")] private bool autoLeveling = true;

    // 現在の速度レベル（データベース参照時に使用）
    private int currentLevel = 0;
    private float levelUpTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("ChibaCorgiController: Rigidbody がアタッチされていません。");
            enabled = false;
            return;
        }

        if (speedDatabase == null)
        {
            Debug.LogWarning("ChibaCorgiController: speedDatabase が設定されていません。frontSpeed をフォールバックとして使用します。");
        }

    }

    void Update()
    {
        if (!autoLeveling || speedDatabase == null) return;

        // タイマー進行。大きなフレーム遅延にも対応して複数段階アップできる。
        levelUpTimer += Time.deltaTime;
        while (levelUpTimer >= levelUpInterval)
        {
            levelUpTimer -= levelUpInterval;
            TryIncreaseLevelByTimer();
            // 最大レベルに達したらタイマーをリセットして抜ける
            if (speedDatabase != null && currentLevel >= speedDatabase.GetMaxLevel())
            {
                levelUpTimer = 0f;
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        // speedDatabase がある場合はデータベースの値を参照し、なければ frontSpeed を使用
        float currentFrontSpeed = speedDatabase != null ? speedDatabase.GetSpeedForLevel(currentLevel) : frontSpeed;

        // 常に前進する速度（重力などのY成分は保持）
        Vector3 forwardVel = transform.forward * currentFrontSpeed;
        forwardVel.y = rb.velocity.y;
        rb.velocity = forwardVel;
    }

    // タイマー由来のレベルアップ（外部呼び出しと分けてログや挙動をわけられる）
    private void TryIncreaseLevelByTimer()
    {
        if (speedDatabase == null) return;
        if (currentLevel < speedDatabase.GetMaxLevel())
        {
            currentLevel++;
            Debug.Log($"ChibaCorgiController: Auto Level Up -> {currentLevel} (speed={speedDatabase.GetSpeedForLevel(currentLevel)})");
        }
    }

    // 自動レベルアップの制御API
    public void SetAutoLeveling(bool enabled)
    {
        autoLeveling = enabled;
        if (!autoLeveling) levelUpTimer = 0f;
    }

    // タイマーをリセット（チェックポイント等で呼ぶ想定）
    public void ResetLevelTimer()
    {
        levelUpTimer = 0f;
    }
}
