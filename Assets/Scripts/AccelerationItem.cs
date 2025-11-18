using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationItem : MonoBehaviour, Items
{
    //Itemsのルールなので、このメソッドを必ず書く
    public void ApplyEffect(GameObject target)
    {
        //渡されてきたGameObject(プレイヤー)からPlayerControllerを取得
        PlayerController player = target.GetComponent<PlayerController>();
        if (player != null)
        {
            // プレイヤーのレベルを上げるメソッドを呼び出す
            player.IncreaseLevel();
        }

        //役目を終えたので自分自身を消す
        Destroy(gameObject);
    }
}
