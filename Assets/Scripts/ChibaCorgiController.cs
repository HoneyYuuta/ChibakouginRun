using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //DOTweenを使うために必要

public class ChibaCorgiController : MonoBehaviour
{
    Rigidbody rb;

    [Header("データベース")]
    [SerializeField] private speedDatabase speedDatabase;
    [SerializeField][Header("前移動速度（フォールバック）")] private float frontSpeed;

    [Header("自動レベルアップ設定")]
    [SerializeField][Tooltip("何秒ごとにレベルを上げるか")] private float levelUpInterval = 20f;
    [SerializeField][Tooltip("自動レベルアップを有効にするか")] private bool autoLeveling = true;
    [SerializeField][Header("チバコーギーに加算する速度")] private float speed = 1f;
    private float speedchiba = 0f;

    [SerializeField][Header("何秒待ってから移動させるか")] private float DelayTime = 5.0f;

    //現在の速度レベル（データベース参照時に使用）
    private int currentLevel = 0;
    private float levelUpTimer = 0f;

    //ゲームが始まったかどうかを管理するフラグ
    private bool isGameStarted = false;

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

        //DOTweenを使って待機処理を行う
        //DelayTime秒待ってから、{}の中身を実行する
        DOVirtual.DelayedCall(DelayTime, () =>
        {
            isGameStarted = true;
            Debug.Log("ChibaCorgi Start!"); //確認用ログ
        })
        .SetLink(gameObject); //オブジェクトが破壊されたらタイマーもキャンセルする安全策
    }

    void Update()
    {
        //まだ始まっていなければ、レベルアップタイマーも動かさない
        if (!isGameStarted) return;

        if (!autoLeveling || speedDatabase == null) return;

        //タイマー進行
        levelUpTimer += Time.deltaTime;
        while (levelUpTimer >= levelUpInterval)
        {
            levelUpTimer -= levelUpInterval;
            TryIncreaseLevelByTimer();
            if (speedDatabase != null && currentLevel >= speedDatabase.GetMaxLevel())
            {
                levelUpTimer = 0f;
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        //まだ始まっていなければ移動させない
        if (!isGameStarted) return;

        //speedDatabase がある場合はデータベースの値を参照し、なければ frontSpeed を使用
        float currentFrontSpeed = speedDatabase != null ? speedDatabase.GetSpeedForLevel(currentLevel) : frontSpeed;

        //常に前進する速度
        Vector3 forwardVel = transform.forward * currentFrontSpeed;
        forwardVel.y = rb.velocity.y;
        rb.velocity = forwardVel;
    }

    //タイマー由来のレベルアップ
    private void TryIncreaseLevelByTimer()
    {
        if (speedDatabase == null) return;
        if (currentLevel < speedDatabase.GetMaxLevel())
        {
            currentLevel++;
            Debug.Log($"ChibaCorgiController: Auto Level Up -> {currentLevel} (speed={speedDatabase.GetSpeedForLevel(currentLevel)})");
        }
    }

    public void SetAutoLeveling(bool enabled)
    {
        autoLeveling = enabled;
        if (!autoLeveling) levelUpTimer = 0f;
    }

    public void ResetLevelTimer()
    {
        levelUpTimer = 0f;
    }
}