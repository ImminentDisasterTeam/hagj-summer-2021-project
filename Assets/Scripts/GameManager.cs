using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType { Menu, Dialogue, Battle }
public class GameManager : MonoBehaviour
{
    private int _partNumber;
    [SerializeField] DialogueSystem.DialogueWindow dialogueWindow;
    void Start()
    {
    }

    void Update()
    {

    }

    void InitiatePlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("music_volume"))
            PlayerPrefs.SetFloat("music_volume", 1);
        if (!PlayerPrefs.HasKey("sfx_volume"))
            PlayerPrefs.SetFloat("sfx_volume", 1);
        if (!PlayerPrefs.HasKey("master_volume"))
            PlayerPrefs.SetFloat("master_volume", 1);
        if (!PlayerPrefs.HasKey("scene_type"))
            PlayerPrefs.SetString("scene_type", "battle");
        if (!PlayerPrefs.HasKey("dialogue"))
            PlayerPrefs.SetInt("dialogue", 0);
        if (!PlayerPrefs.HasKey("battle"))
            PlayerPrefs.SetInt("battle", 0);
    }
}
