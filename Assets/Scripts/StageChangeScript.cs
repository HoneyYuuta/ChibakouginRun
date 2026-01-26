using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using DG.Tweening;

public class StageChangeScript : MonoBehaviour
{
    [SerializeField] 
     TextMeshProUGUI stageText;
    //画像
    [SerializeField]
    UnityEngine.UI.Image stageImage;
    [SerializeField]
    float fadeDuration = 1f;//フェードの時間
    [SerializeField]
    float displayDuration = 2f;//表示時間
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        
    }
    public void StageUpdate(string stageName)
    {
        if (stageText != null)
        {
            stageText.text = stageName;
        }
        this.gameObject.SetActive(true);
        var fadeIn = DOTween.Sequence();
        fadeIn.Append(stageImage.DOFade(0, fadeDuration).SetDelay(displayDuration))
            .Join(stageText.DOFade(0, fadeDuration))
        .OnComplete(() => 
        {
            this.gameObject.SetActive(false);
            stageImage.DOFade(1, 0);
            stageText.DOFade(1, 0);
        });
    }
}
