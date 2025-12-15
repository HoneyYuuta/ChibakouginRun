using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("UI参照")]
    [SerializeField] private TMP_InputField nameInputField; // 名前入力欄
    [SerializeField] private Button startButton;            // スタートボタン


    // Start is called before the first frame update
    void Start()
    {
        // 1. 最初はボタンを押せないようにする
        startButton.interactable = false;

        // 2. InputFieldの文字が変わるたびに呼ばれるイベントを登録
        nameInputField.onValueChanged.AddListener(OnNameChanged);
    }

    // Update is called once per frame
    private void OnNameChanged(string text)
    {
        // 文字列が「空」または「空白のみ」でないかチェック
        bool hasText = !string.IsNullOrWhiteSpace(text);

        // 文字があれば true (押せる)、なければ false (押せない) にする
        startButton.interactable = hasText;
    }
}
