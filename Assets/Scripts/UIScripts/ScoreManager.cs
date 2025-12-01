using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //どこからでもアクセスできるようにする（シングルトン）
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;

    //表示用の合計スコア
    private float totalScore = 0;

    //アイテムなどで獲得したボーナススコア
    private float bonusScore = 0;

    [SerializeField] private GameObject PlayerObj;

    private void Awake()
    {
        //シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (PlayerObj == null) return;

        //合計 = プレイヤーのZ座標(距離) + ボーナススコア
        float distanceScore = PlayerObj.transform.position.z;
        totalScore = distanceScore + bonusScore;

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText == null) return;

        //小数点を切り捨てて表示
        int displayScore = Mathf.FloorToInt(totalScore);
        scoreText.text = "Score:" + displayScore.ToString();
    }

    //アイテムからスコアを加算するメソッド
    public void AddScore(int amount)
    {
        bonusScore += amount;
        Debug.Log("Bonus Added! Current Bonus: " + bonusScore);
    }
}
