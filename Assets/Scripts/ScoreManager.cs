using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private float score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 初期スコアを表示
        UpdateScoreText();
        score += Time.deltaTime;
    }

    void UpdateScoreText()
    {
        if (scoreText == null) return;
        // 小数点を切り捨てて表示
        int displayScore = Mathf.FloorToInt(score);
        scoreText.text = "Score: " + displayScore.ToString();
    }
}
