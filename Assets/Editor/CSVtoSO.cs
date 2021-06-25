using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using DialogueSystem;

public class CSVtoSO
{
    private static string csvPath = "/Editor/CSVs/";
    private static string spritesPath = "/Sprites/";
    private static char SplitSymbol = ',';

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
        dialogue.Title = Path.GetFileNameWithoutExtension(fileaPath);


        dialogue.Phrases = new List<Phrase>();
        for (int i = 3; i < allLines.Length; i++)
        {
            Phrase phrase = CreatePhrase(SplitLine(allLines[i]));
            dialogue.Phrases.Add(phrase);
        }

        AssetDatabase.CreateAsset(dialogue, $"Assets/Dialogs/{dialogue.Title}.asset");
    }

    private static string[] SplitLine(string line)
    {
        string[] result = new string[5];
        bool inText = false;
        int startIndex = 0;
        int substringLength = 0;
        int resultIndex = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == SplitSymbol && line[i - 1] == '\"')
            {
                substringLength--;
                result[resultIndex] = line.Substring(startIndex + 1, substringLength - 2);
                Debug.Log(result[resultIndex]);
                resultIndex++;
                startIndex = i + 1;
                substringLength = -1;

                inText = false;
            }
            else if (line[i] == '\"' && line[i - 1] == SplitSymbol)
            {
                inText = true;
            }
            else if (!inText && line[i] == SplitSymbol)
            {
                result[resultIndex] = line.Substring(startIndex, substringLength);
                Debug.Log(result[resultIndex]);
                resultIndex++;
                startIndex = i + 1;
                substringLength = 0;
            }
            substringLength++;
        }
        return result;
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
            string path = Application.dataPath + spritesPath + allData[data.Length + i] + ".png";
            sprites[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
            //Debug.Log(path);
        }

        return new Phrase(data, sprites);
    }

}
