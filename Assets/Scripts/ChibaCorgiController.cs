using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChibaCorgiController : MonoBehaviour
{
    Rigidbody rb;

    //プレイヤーの位置を知るための変数
    private Transform playerTransform;

    //アクションを1回だけ実行するためのフラグ
    private bool isActionDone = false;

    [Header("ゲームオーバー演出")]
    [SerializeField] private float offsetDistance = 3.0f; //何メートル後ろに出るか
    [SerializeField] private float jumpPower = 2.0f;      //ジャンプの高さ
    [SerializeField] private float duration = 0.5f;       //アニメーション時間

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //ゲーム開始時にプレイヤーを探して確保しておく
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("PlayerControllerが見つかりませんでした！");
        }
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver && !isActionDone)
        {
            GameOverAction();
        }
    }

    private void GameOverAction()
    {
        isActionDone = true;

        if (playerTransform == null) return;

        Debug.Log("チバコーギーのGameOver処理開始");

        //物理挙動を完全に止める
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true; //物理演算を無効化してDOTweenで動かせるようにする
        }

        //プレイヤーの背後にワープ
        Vector3 warpPos = playerTransform.position - (playerTransform.forward * offsetDistance);

        //高さは今のチバコーギーの高さを維持
        warpPos.y = transform.position.y;

        transform.position = warpPos;

        //プレイヤーの方を向く
        transform.LookAt(playerTransform);

        //飛びかかるアクション
        transform.DOJump(playerTransform.position, jumpPower, 1, duration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                Debug.Log("捕まえたワン！");
            });
    }
}