using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationItem : MonoBehaviour, Items
{
    //Itemsのルールなので、このメソッドを必ず書く

    [Header("効果時間（秒）")]
    [SerializeField] private float duration = 5.0f; // 一定時間スピードアップ

    public void ApplyEffect(GameObject target)
    {
        //渡されてきたGameObject(プレイヤー)からPlayerControllerを取得
        PlayerController player = target.GetComponent<PlayerController>();
        if (player != null)
        {
            // 以前の IncreaseLevel() ではなく、時間指定版のメソッドを呼ぶ
            player.ApplyTemporarySpeedUp(duration);
        }

        //役目を終えたので自分自身を消す
        gameObject.SetActive(false);
    }
}
