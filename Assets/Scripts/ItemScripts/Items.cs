using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//取得可能なアイテムが実装すべきインターフェース
public interface Items
    {
        //アイテムの効果を対象に適用する
        void ApplyEffect(GameObject target);
    }
