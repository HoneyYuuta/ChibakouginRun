using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //DOTween‚ðŽg‚¤‚½‚ß‚É•K—v

public class ChibaCorgiController : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            GameOver();
        }
    }

    private void GameOver()
    {

    }


}