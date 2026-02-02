using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSEController : MonoBehaviour
{
    [SerializeField] private AudioSource SE;    //SE再生用AudioSource 

    [SerializeField] private AudioClip ScoreUpSE;  //スコア上昇系アイテム取得時SE
    [SerializeField] private AudioClip DamageSE;   //ダメージSE
    [SerializeField] private AudioClip MoveSE;
    public void PlayScoreUpSE()
    {
        SE.PlayOneShot(ScoreUpSE);
    }

    public void PlayDamageSE()
    {
        SE.PlayOneShot(DamageSE);
    }
    public void PlayMoveSE()
    {
        SE.PlayOneShot(MoveSE);
    }
}
