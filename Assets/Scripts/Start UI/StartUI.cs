using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StartUI : MonoBehaviour
{
    [SerializeField]
    GameObject startUI;
    //テキスト
    [SerializeField]
    TextMeshProUGUI startText;

    //カウントダウン時間
    [SerializeField]
    float countDownTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCountDown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //カウントダウン開始
    public void StartCountDown()
    {
        startUI.SetActive(true);
        var fadeIn = DOTween.Sequence();
        //３秒カウントダウン
        fadeIn.Append(startText.DOFade(0, countDownTime).SetDelay(countDownTime))
            .AppendCallback(() =>
            {
                startText.DOFade(1, 0);
                startText.text = "2";
            }).
            Append(startText.DOFade(0, countDownTime).SetDelay(countDownTime))
            .AppendCallback(() =>
            {
                startText.DOFade(1, 0);
                startText.text = "1";
            }).
            Append(startText.DOFade(0, countDownTime).SetDelay(countDownTime))
            .AppendCallback(() =>
            {
                startText.DOFade(1, 0);
                startText.text = "Start!";
            }).
            Append(startText.DOFade(0, countDownTime).SetDelay(countDownTime))
            .AppendCallback(() =>
            {
                startUI.SetActive(false);
            });


    }
}
