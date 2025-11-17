using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField][Header("Œo‰ßŽžŠÔ")] private float ElapsedTime = 0f;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ChibaCorgi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ElapsedTime += Time.deltaTime;

        var playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.FrontSpeed = 5 + (ElapsedTime / 10);
        }
    }
}
