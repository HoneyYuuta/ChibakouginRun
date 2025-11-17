using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationItem : MonoBehaviour
{
    [SerializeField][Header("加速の効果時間")] private float AccelerationDuration;
    [SerializeField][Header("速度の増加量")] private float SpeedIncrease;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.FrontSpeed += 2.0f; // 速度を2.0f増加させる
                Destroy(gameObject); // アイテムを消す
            }
        }
        */
    }

}
