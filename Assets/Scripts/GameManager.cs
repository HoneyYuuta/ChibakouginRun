using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //どこからでも呼べるようにする

    public bool IsGameOver { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    //ゲームオーバー処理
    public void GameOver()
    {
        if (IsGameOver) return; //すでにゲームオーバーなら何もしない

        IsGameOver = true;
        Debug.Log("GAMEOVER判定");

    }
}
