using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingScript : MonoBehaviour
{
    //ランキングテキスト
    [SerializeField]
    TextMeshProUGUI[] rankText;
    

    // Start is called before the first frame update
    void Start()
    {
        DisplayRanking();
    }
    //ランキング表示
    public void DisplayRanking()
    {
        for (int i = 0; i < rankText.Length; i++)
        {
            int score = SaveData.SetRankingScore(i);
            rankText[i].text = score + " " + "点";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
