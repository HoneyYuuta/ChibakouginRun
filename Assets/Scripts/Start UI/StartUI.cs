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
        Time.timeScale = 0;
        var fadeIn = DOTween.Sequence().SetUpdate(true);
        startText.DOFade(1, 0).SetUpdate(true);
        //３秒カウントダウン
        fadeIn.Append(startText.DOFade(0, countDownTime).SetDelay(countDownTime))
            .AppendCallback(() =>
            {
                startText.DOFade(1, 0).SetUpdate(true);
                startText.text = "2";
            }).
            Append(startText.DOFade(0, countDownTime).SetDelay(countDownTime))
            .AppendCallback(() =>
            {
                startText.DOFade(1, 0).SetUpdate(true);
                startText.text = "1";
            }).
            Append(startText.DOFade(0, countDownTime).SetDelay(countDownTime))
            .AppendCallback(() =>
            {
                startText.DOFade(1, 0).SetUpdate(true);
                startText.text = "Start!";
                Time.timeScale = 1;
            }).
            Append(startText.DOFade(0, countDownTime).SetDelay(countDownTime))
            .AppendCallback(() =>
            {
                startText.text = "3";
                startUI.SetActive(false);
            });


    }
    //中断
    public void StopCountDown()
    {
        DOTween.CompleteAll(true);
        startText.text = "3";
        startUI.SetActive(false);
        Time.timeScale = 0;
    }
}
