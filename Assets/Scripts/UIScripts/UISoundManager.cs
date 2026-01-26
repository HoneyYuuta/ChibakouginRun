using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class UISoundManager : MonoBehaviour
{
    [Header("効果音")]
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;

    void Start()
    {
        //AudioSourceの準備
        audioSource = GetComponent<AudioSource>();

        //シーン内にある「全てのボタン」を探し出す
        Button[] allButtons = FindObjectsOfType<Button>();

        //全てのボタンに「音を鳴らす処理」を追加する
        foreach (Button btn in allButtons)
        {
            //すでに登録済みでないか確認（重複防止）
            btn.onClick.RemoveListener(PlayClickSound);

            //クリック時の処理に「音を鳴らす」を追加
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    //音を鳴らすメソッド
    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            //PlayOneShotなら連続で押しても音が重なって綺麗に聞こえる
            audioSource.PlayOneShot(clickSound);
        }
    }
}