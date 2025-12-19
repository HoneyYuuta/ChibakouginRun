using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    // AudioMixerとSliderをSerializeFieldで取得する
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;

    private void Start()
    {
        float bgmVolume;
        float seVolume;
        //BGM
        audioMixer.GetFloat("BGM",out bgmVolume);
        BGMSlider.value = bgmVolume;
        //SE
        audioMixer.GetFloat("SE",out seVolume);
        SESlider.value = seVolume;
    }

    // スライダーの値をdB（デシベル）に変換して、AudioMixerの「BGM」パラメータに反映させる
    public void OnBGMVolumeChanged(float volume)
    {
        volume = BGMSlider.value;
        audioMixer.SetFloat("BGM", volume);
    }

    // スライダーの値をdB（デシベル）に変換して、AudioMixerの「SE」パラメータに反映させる
    public void OnSEVolumeChanged(float volume)
    {
        volume = SESlider.value;
        audioMixer.SetFloat("SE", volume);
    }
}
