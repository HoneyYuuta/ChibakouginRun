using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 追従する対象（プレイヤー）のTransformをインスペクターから設定する
    [SerializeField] private Transform player;

    // カメラとプレイヤーのZ軸の距離を保つための変数
    private float offsetZ;

    void Start()
    {
        // ゲーム開始時に、カメラとプレイヤーのZ軸の初期距離を計算して保存する
        if (player != null)
        {
            offsetZ = transform.position.z - player.position.z;
        }
        else
        {
            Debug.LogError("Player transform is not set on the camera script.");
        }
    }

    void LateUpdate()
    {
        // プレイヤーが設定されている場合のみ処理を行う
        if (player != null)
        {
            // カメラの新しい位置を計算する
            // 現在のカメラのX, Y座標はそのまま使い、Z座標だけを更新する
            Vector3 newPosition = transform.position;
            newPosition.z = player.position.z + offsetZ;

            // 計算した新しい位置をカメラの座標に設定する
            transform.position = newPosition;
        }
    }
}
