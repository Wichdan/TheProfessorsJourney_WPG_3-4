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
        MusicManager.singleton.SetBGMSliderValue(musicSlider);
        MusicManager.singleton.SetSFXSliderValue(sfxSlider);

        saveBtn.onClick.AddListener(() =>
        {
            MusicManager.singleton.ApplyBGMVolume(musicSlider);
            MusicManager.singleton.ApplySFXVolume(sfxSlider);
        });

        resetBtn.onClick.AddListener(() =>
        {
            MusicManager.singleton.ResetVolume();
            MusicManager.singleton.SetBGMSliderValue(musicSlider);
            MusicManager.singleton.SetSFXSliderValue(sfxSlider);
        });
    }
}
