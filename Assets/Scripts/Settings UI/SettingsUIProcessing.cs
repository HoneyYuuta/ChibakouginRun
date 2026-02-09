using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUIProcessing : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject UI;
    [SerializeField]
    GameObject settingUI;
    [SerializeField]
    GameObject volumeUI;

    [SerializeField][Header("StartUIのオブジェクト")] private StartUI startUI;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void WhenBackIsPressed()
    {
        UI.SetActive(true);
        settingUI.SetActive(false);
        volumeUI.SetActive(false);
        Time.timeScale = 1;
        startUI.StartCountDown();
    }
    //設定が押されたとき
    public void WhenSettingsIsPressed()
    {

        UI.SetActive(false);
        settingUI.SetActive(true);
        volumeUI.SetActive(false);
        startUI.StopCountDown();
        Time.timeScale = 0;
    }
    //音量が押されたとき
    public void WhenVolumeIsPressed()
    {
        UI.SetActive(false);
        settingUI.SetActive(false);
        volumeUI.SetActive(true);
    }




}
