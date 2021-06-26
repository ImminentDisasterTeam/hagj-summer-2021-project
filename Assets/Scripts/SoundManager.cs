using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource _sfxSource;
    [SerializeField] AudioSource _musicSource;
    private float _masterVolume;
    public void ChangeSliderVolume(Slider slider)
    {
        PlayerPrefs.SetFloat(slider.name, slider.value);
        PlayerPrefs.Save();
        ApplySliderVolumeChange(slider.name, slider);
    }
    private void ApplySliderVolumeChange(string soundType, Slider slider)
    {
        float masterVolume = PlayerPrefs.GetFloat("master_volume");
        switch (soundType)
        {
            case "music_volume":
                _musicSource.volume = masterVolume * slider.value;
                break;
            case "sfx_volume":
                _sfxSource.volume = masterVolume * slider.value;
                break;
            case "master_volume":
                _musicSource.volume = masterVolume * slider.value;
                _sfxSource.volume = masterVolume * slider.value;
                break;
            default:
                break;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        _sfxSource.volume = PlayerPrefs.GetFloat("sfx_volume");
        _sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.volume = PlayerPrefs.GetFloat("music_volume");
        Debug.Log(_musicSource.volume);
        _musicSource.clip = clip;
        _musicSource.Play();
    }
}
