using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private const string mainMenuSceneName = "MainMenu";
    private const string dialogueSceneName = "Dialogues";
    public void LoadSceneByName(string sceneName)
    {
        if (sceneName == mainMenuSceneName || sceneName == dialogueSceneName)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        SceneManager.LoadScene(sceneName);
    }
}
