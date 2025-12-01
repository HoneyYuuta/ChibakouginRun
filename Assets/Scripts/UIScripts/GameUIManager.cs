using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private PlayerController playerController;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Slider buffSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //スコアの表示更新 (ScoreManagerからデータを貰う)
        if (ScoreManager.Instance != null && scoreText != null)
        {
            //小数点を切り捨てて表示
            int displayScore = Mathf.FloorToInt(ScoreManager.Instance.TotalScore);
            scoreText.text = "Score: " + displayScore.ToString();
        }

        //速度の表示更新
        if (playerController != null && speedText != null)
        {
            speedText.text = "Speed: " + playerController.CurrentSpeed.ToString("F0") + " km/h";
        }

        //ゲージの表示更新
        if (playerController != null && buffSlider != null)
        {
            float ratio = playerController.BuffTimeRatio;
            buffSlider.value = ratio;

            if (ratio > 0) buffSlider.gameObject.SetActive(true);
            else buffSlider.gameObject.SetActive(false);
        }
    }
}
