using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CSVtoSO
{
    private static string csvPath = "/Editor/CSVs/";

    [MenuItem("Utilities/Generate Dialogs")]
    public static void GenerateDialogs()
    {
        var files = Directory.GetFiles(Application.dataPath + csvPath,"*.csv");
        foreach (var file in files)
        {
            Debug.Log(file);
            GenerateDialog(file);
        }

        AssetDatabase.SaveAssets();
    }
    private static void GenerateDialog(string fileaPath)
    {
        string[] allLines = File.ReadAllLines(fileaPath);
        Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
        dialogue.phrases = new List<Phrase>();
        dialogue.Title = "test";
        foreach (var line in allLines)
        {
            Phrase phrase = new Phrase(line);
            dialogue.phrases.Add(phrase);
        }


        AssetDatabase.CreateAsset(dialogue, $"Assets/Dialogs/{dialogue.Title}.asset");
    }
}
