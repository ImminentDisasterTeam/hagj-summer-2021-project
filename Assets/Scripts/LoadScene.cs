using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
    public const string MainMenu = "MainMenu";
    public const string Dialogue = "Dialogues";
    public const string Battle1 = "Battle1";
    public const string Battle2 = "Battle2";
    public const string Battle3 = "Battle3";

    public void NewGame() {
        PlayerPrefs.SetInt("dialogue", 0);
        LoadSceneByName(Dialogue);
    }
    
    public void LoadSceneByName(string sceneName) {
        
        SceneManager.LoadScene(sceneName);
    }
}
