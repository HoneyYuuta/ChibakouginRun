using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpItem : MonoBehaviour , Items
{
    //このアイテムを取った時のスコア
    [Header("獲得スコア")]
    [SerializeField] private int scoreAmount = 100;
public void ApplyEffect(GameObject target)
    {
        //ScoreManagerが存在するか確認してから加算する
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreAmount);
        }

        //役目を終えたので自分自身を消す
        gameObject.SetActive(false);
    }
}
