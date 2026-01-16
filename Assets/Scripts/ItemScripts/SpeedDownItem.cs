using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDownItem : MonoBehaviour, Items
{
    [Header("効果時間（秒）")]
    [SerializeField] private float duration = 3.0f; // 3秒間減速

    public void ApplyEffect(GameObject target)
    {
        // プレイヤーを取得
        PlayerController player = target.GetComponent<PlayerController>();

        if (player != null)
        {
            //一時的な減速メソッドを呼ぶ
            player.ApplyTemporarySpeedDown(duration);
        }

        gameObject.SetActive(false);
    }
}
