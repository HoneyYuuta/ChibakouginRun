using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//取得可能なアイテムが実装すべきインターフェース（ルール）
public interface Items
    {
        //アイテムの効果を対象に適用する
        //<param name="target">効果を適用する対象のGameObject (この場合はプレイヤー)</param>
        void ApplyEffect(GameObject target);
    }
