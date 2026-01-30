using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIButtonAnimManager : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float pressScale = 1.1f; // 押した時の倍率
    [SerializeField] private float duration = 0.1f;   // アニメーション時間

    void Start()
    {
        // シーン中の「Button」コンポーネントがついている物を全部探す
        // (TMPのボタンもButtonコンポーネントがついているので検知されます)
        Button[] allButtons = FindObjectsOfType<Button>();

        foreach (Button btn in allButtons)
        {
            // すでにアニメーション機能がついていなければ追加する
            if (btn.gameObject.GetComponent<SimplePressAnim>() == null)
            {
                var anim = btn.gameObject.AddComponent<SimplePressAnim>();
                anim.Setup(pressScale, duration);
            }
        }
    }
}

// ▼ 自動でボタンにくっつくアニメーション用部品 ▼
public class SimplePressAnim : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float targetScale;
    private float duration;
    private Vector3 originalScale;
    private Tween tween;

    public void Setup(float scale, float time)
    {
        this.targetScale = scale;
        this.duration = time;
        this.originalScale = transform.localScale;
    }

    // 押した時：拡大
    public void OnPointerDown(PointerEventData eventData)
    {
        tween?.Kill();
        tween = transform.DOScale(originalScale * targetScale, duration)
            .SetEase(Ease.OutQuad)
            .SetLink(gameObject);
    }

    // 離した時：元に戻る
    public void OnPointerUp(PointerEventData eventData)
    {
        tween?.Kill();
        tween = transform.DOScale(originalScale, duration)
            .SetEase(Ease.OutQuad)
            .SetLink(gameObject);
    }
}
