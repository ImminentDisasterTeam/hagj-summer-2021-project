using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAsset
{
    [UnityEditor.MenuItem("Assets/Create/Dialogue")]
    public static void CreateAsset()
    {
        CustomAssetUtility.CreateAsset<Dialogue>();
    }
}
