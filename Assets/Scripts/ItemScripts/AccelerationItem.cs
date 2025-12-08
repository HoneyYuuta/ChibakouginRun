using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationItem : MonoBehaviour, Items
{
    [Header("効果時間（秒）")]
    [SerializeField] private float duration = 3.0f; //一定時間スピードアップ

    //このアイテムを取った時のスコア
    [Header("獲得スコア")]
    [SerializeField] private int scoreAmount = 100;

    public void ApplyEffect(GameObject target)
    {
        //プレイヤーへの速度変更効果
        PlayerController player = target.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplyTemporarySpeedUp(duration);
        }

        //ScoreManagerが存在するか確認してから加算する
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreAmount);
        }

        //役目を終えたので自分自身を消す
        gameObject.SetActive(false);
    }
}
