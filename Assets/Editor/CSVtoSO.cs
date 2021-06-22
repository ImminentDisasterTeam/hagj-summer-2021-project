using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using DialogueSystem;

public class CSVtoSO
{
    private static string csvPath = "/Editor/CSVs/";

    [MenuItem("Utilities/Generate Dialogs")]
    public static void GenerateDialogs()
    {
        var files = Directory.GetFiles(Application.dataPath + csvPath, "*.csv");
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
        dialogue.Phrases = new List<Phrase>();
        dialogue.Title = Path.GetFileNameWithoutExtension(fileaPath);
        foreach (var line in allLines)
        {
            if ((line.Split(Phrase.SplitSymbol)).GetLength(0) == 4)
            {
                Phrase phrase = new Phrase(line);
                dialogue.Phrases.Add(phrase);
            }
        }

        AssetDatabase.CreateAsset(dialogue, $"Assets/Dialogs/{dialogue.Title}.asset");
    }
}
