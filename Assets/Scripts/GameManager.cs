using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SceneType { Menu, Dialogue, Battle }
public class GameManager : MonoBehaviour
{
    [SerializeField] DialogueSystem.DialogueWindow dialogueWindow;
    [SerializeField] BattleManager battleManager;

    static readonly Dictionary<int, string> LastDialogueInSceneNumbers = new Dictionary<int, string> {
        {4, LoadScene.Battle1},
        {7, LoadScene.Battle2},
        {9, LoadScene.Battle3}
    };

    const int LastDialogueInGameNumber = 11;

    void Start() {
        InitiatePlayerPrefs();

        var loadScene = GetComponent<LoadScene>();
        
        if (dialogueWindow != null) {
            dialogueWindow.NextScene += () => {
                var dialogueNumber = PlayerPrefs.GetInt("dialogue");
                Debug.Log($"A: {dialogueNumber}");
                PlayerPrefs.SetInt("dialogue", dialogueNumber + 1);
                
                if (LastDialogueInSceneNumbers.ContainsKey(dialogueNumber)) {
                    Debug.Log("B");
                    loadScene.LoadSceneByName(LastDialogueInSceneNumbers[dialogueNumber]);
                    return;
                }

                if (LastDialogueInGameNumber == dialogueNumber) {
                    Debug.Log("C");
                    // TODO: show credits
                    loadScene.LoadSceneByName(LoadScene.MainMenu);
                    return;
                }
                
                Debug.Log("D");
                dialogueWindow.StartDialogue();
            };
            
            dialogueWindow.StartDialogue();
            return;
        }

        if (battleManager != null) {
            battleManager.OnWin += () => {
                loadScene.LoadSceneByName(LoadScene.Dialogue);
            };
        }
    }

    void InitiatePlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("music_volume"))
            PlayerPrefs.SetFloat("music_volume", 1);
        if (!PlayerPrefs.HasKey("sfx_volume"))
            PlayerPrefs.SetFloat("sfx_volume", 1);
        if (!PlayerPrefs.HasKey("master_volume"))
            PlayerPrefs.SetFloat("master_volume", 1);
        if (!PlayerPrefs.HasKey("dialogue"))
            PlayerPrefs.SetInt("dialogue", 0);
    }
}
