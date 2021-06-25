using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class DialogueAsset
{
    [UnityEditor.MenuItem("Assets/Create/Dialogue")]
    public static void CreateAsset()
    {
        CustomAssetUtility.CreateAsset<Dialogue>();
    }
}
