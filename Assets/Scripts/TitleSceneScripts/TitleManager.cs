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
    [SerializeField] private TextMeshProUGUI BottonText;              // ボタンテキスト

    [Header("表示するメッセージ")]
    [SerializeField] private string emptyMessage = "先に名前を入力してね！";
    [SerializeField] private string validMessage = "GAME START";


    // Start is called before the first frame update
    void Start()
    {
        //最初はボタンを押せないようにする
        startButton.interactable = false;

        //InputFieldの文字が変わるたびに呼ばれるイベントを登録
        nameInputField.onValueChanged.AddListener(UpdateUIState);

        UpdateUIState(nameInputField.text);

        string savedName = SaveData.SetName();

        if (savedName != "Player")
        {
            nameInputField.text = savedName;
        }
    }

    // Update is called once per frame
    private void UpdateUIState(string text)
    {
        // 文字が入っているかチェック
        bool hasText = !string.IsNullOrWhiteSpace(text);

        // 1. ボタンの有効/無効切り替え
        startButton.interactable = hasText;

        // 2. テキストの内容切り替え
        if (BottonText != null)
        {
            if (hasText)
            {
                // 文字がある時
                BottonText.text = validMessage;
            }
            else
            {
                // 文字がない時
                BottonText.text = emptyMessage;
            }
        }
    }

    //ゲーム終了:ボタンから呼び出す
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }

}
