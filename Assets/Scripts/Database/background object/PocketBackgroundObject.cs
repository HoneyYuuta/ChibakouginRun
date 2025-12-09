using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PocketBackgroundObject
{
    public GameObject BackgroundObject;//背景オブジェクト
    public Vector3 BackgroundObjectCoordinate;//背景オブジェクトの座標
    public int BackgroundObjectOrder;//背景オブジェクトの描画順
}
