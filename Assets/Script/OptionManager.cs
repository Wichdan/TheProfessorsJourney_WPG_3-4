using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField] Button saveBtn, resetBtn;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Start()
    {
        MusicManager.instance.SetBGMSliderValue(musicSlider);
        MusicManager.instance.SetSFXSliderValue(sfxSlider);

        saveBtn.onClick.AddListener(() =>
        {
            MusicManager.instance.ApplyBGMVolume(musicSlider);
            MusicManager.instance.ApplySFXVolume(sfxSlider);
        });

        resetBtn.onClick.AddListener(() =>
        {
            MusicManager.instance.ResetVolume();
            MusicManager.instance.SetBGMSliderValue(musicSlider);
            MusicManager.instance.SetSFXSliderValue(sfxSlider);
        });
    }
}
