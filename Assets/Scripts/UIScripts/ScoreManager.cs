using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TextMeshProUGUI addedScoreText;

    //GameUIManagerから読めるようにpublicプロパティにする
    public float TotalScore { get; private set; } = 0;

    private float bonusScore = 0;

    [SerializeField] private GameObject PlayerObj;
    [SerializeField] private PlayerSpeedHandler speedHandler;

    int level = 0;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        // 最初は上昇分テキストを隠しておく
        if (addedScoreText != null)
        {
            addedScoreText.alpha = 0f;
        }
    }

    void Update()
    {
        if (PlayerObj == null) return;

        // スコア計算だけを行う
        float distanceScore = PlayerObj.transform.position.z;
        TotalScore = distanceScore + bonusScore;

    }

    public void AddScore(int amount)
    {
        if (speedHandler != null)
        {
            level = speedHandler.CurrentLevel;
        }

        bonusScore += amount + level * 100;
        ScoreAnimetion(amount);
    }

    private void ScoreAnimetion(int amount)
    {
        if (addedScoreText == null) return;

        // テキストを更新 ("+100" のようにする)
        addedScoreText.text = "+" + amount.ToString();

        // アニメーションのリセット（連打された時用）
        addedScoreText.DOKill(); //前の動きを止める
        addedScoreText.alpha = 1f; //透明度を戻す
        addedScoreText.transform.localScale = Vector3.one; //大きさを戻す

        //ポンッと拡大して出る
        addedScoreText.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f);

        //フェードアウトしながら消える
        addedScoreText.DOFade(0f, 1.0f).SetEase(Ease.InQuart).SetDelay(0.5f);
    }

}