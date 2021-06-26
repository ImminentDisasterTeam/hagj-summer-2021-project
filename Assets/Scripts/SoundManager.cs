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
        switch (soundType)
        {
            case "music_volume":
                _musicSource.volume = _masterVolume * slider.value;
                break;
            case "sfx_volume":
                _sfxSource.volume = _masterVolume * slider.value;
                break;
            case "master_volume":
                _musicSource.volume = PlayerPrefs.GetFloat("music_volume") * slider.value;
                _sfxSource.volume = PlayerPrefs.GetFloat("sfx_volume") * slider.value;
                break;
            default:
                break;
        }
        Debug.Log(PlayerPrefs.GetFloat("master_volume"));
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

    private void Start()
    {
        _masterVolume = PlayerPrefs.GetFloat("master_volume");
        _musicSource.volume = PlayerPrefs.GetFloat("music_volume") * _masterVolume;
        _sfxSource.volume = PlayerPrefs.GetFloat("sfx_volume") * _masterVolume;
    }
}
