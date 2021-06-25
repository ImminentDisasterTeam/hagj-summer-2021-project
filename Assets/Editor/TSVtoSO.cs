using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using DialogueSystem;

public class TSVtoSO
{
    private static string tsvPath = "/Editor/TSVs/";
    private static string spritesPath = "/Sprites/";
    private static string backgroundPath = spritesPath + "Backgrounds/";
    private static string audioPath = "/Sounds/Music/";
    private static char SplitSymbol = '\t';

    [MenuItem("Utilities/Generate Dialogs")]
    public static void GenerateDialogs()
    {
        var files = Directory.GetFiles(Application.dataPath + tsvPath, "*.tsv");
        foreach (var file in files)
        {
            GenerateDialog(file);
        }

        AssetDatabase.SaveAssets();
    }
    private static void GenerateDialog(string fileaPath)
    {
        string[] allLines = File.ReadAllLines(fileaPath);
        Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
        dialogue.Title = Path.GetFileNameWithoutExtension(fileaPath);
        string[] generalData = allLines[1].Split(SplitSymbol);

        dialogue.BackgroundMusic = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>("Assets" + audioPath + generalData[1] + ".ogg");
        dialogue.Background = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets" + backgroundPath + generalData[0] + ".png");

        dialogue.Phrases = new List<Phrase>();
        for (int i = 3; i < allLines.Length; i++)
        {
            Phrase phrase = CreatePhrase((allLines[i]).Split(SplitSymbol));
            dialogue.Phrases.Add(phrase);
        }

        AssetDatabase.CreateAsset(dialogue, $"Assets/Dialogs/{dialogue.Title}.asset");
    }
    private static Phrase CreatePhrase(string[] allData)
    {
        string[] data = new string[3];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = allData[i];
        }

        Sprite[] sprites = new Sprite[2];
        for (int i = 0; i < sprites.Length; i++)
        {
            string path = "Assets" + spritesPath + allData[data.Length + i] + ".png";
            sprites[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        return new Phrase(data, sprites);
    }

}
