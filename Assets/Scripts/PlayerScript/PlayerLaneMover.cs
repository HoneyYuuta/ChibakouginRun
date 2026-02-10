using UnityEngine;
using DG.Tweening;

//プレイヤーの横移動（レーンチェンジ）のみを管理するコンポーネント

public class PlayerLaneMover : MonoBehaviour
{
    [SerializeField][Header("横移動にかける時間")] private float lateralMoveDuration = 0.2f;
    [SerializeField][Header("横移動の距離（レーン幅）")] private float lateralMoveDistance = 1.5f;

    //現在のレーン位置 (-1:左, 0:中央, 1:右)
    private int currentLaneIndex = 0;
    //移動中フラグ
    private bool isLaneChanging = false;

    //左へ移動リクエスト
    public void MoveLeft()
    {
        if (isLaneChanging || currentLaneIndex <= -1) return;
        currentLaneIndex--;
        ExecuteMove();
    }

    //右へ移動リクエスト
    public void MoveRight()
    {
        if (isLaneChanging || currentLaneIndex >= 1) return;
        currentLaneIndex++;
        ExecuteMove();
    }

    //実際の移動処理
    private void ExecuteMove()
    {
        if (GameManager.Instance.IsGameOver) return;
        PlayerSEController playerSEController = GetComponent<PlayerSEController>();
        if (playerSEController != null)
        {
            playerSEController.PlayMoveSE();
        }

        isLaneChanging = true;
        float targetX = currentLaneIndex * lateralMoveDistance;

        transform.DOLocalMoveX(targetX, lateralMoveDuration)
            .SetEase(Ease.OutQuad)
            .SetLink(gameObject)
            .OnComplete(() => isLaneChanging = false);
    }
}