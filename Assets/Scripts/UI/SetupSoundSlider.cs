using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupSoundSlider : MonoBehaviour
{
    private void OnEnable() {
        Slider slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat(slider.name);
        Debug.Log(PlayerPrefs.GetFloat(slider.name));
    }
}
