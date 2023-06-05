using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource bgMusic;
    [SerializeField] AudioSource soundEffect;

    [SerializeField] List<AudioClip> bgMusics;

    public static MusicManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ApplyBGMVolume(Slider slider) => bgMusic.volume = slider.value;

    public void ApplySFXVolume(Slider slider) => soundEffect.volume = slider.value;

    public void SetBGMSliderValue(Slider slider) => slider.value = bgMusic.volume;
    public void SetSFXSliderValue(Slider slider) => slider.value = soundEffect.volume;

    public void ResetVolume()
    {
        bgMusic.volume = 1;
        soundEffect.volume = 1;
    }

    public void SetAndPlayBGM(int bgmIndex)
    {
        bgMusic.clip = bgMusics[bgmIndex];
        bgMusic.Play();
    }

    public void SetAndPlaySFX(AudioClip sfx)
    {
        soundEffect.clip = sfx;
        soundEffect.Play();
    }

    public void StopBGM() => bgMusic.Stop();
}
