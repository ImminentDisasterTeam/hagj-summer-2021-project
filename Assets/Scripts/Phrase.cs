using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Position { None, Left, Right }

[System.Serializable]
public class Phrase
{
    public Phrase(string data)
    {
        var splitData = data.Split(';');
        CharacterName = splitData[0];
        Text = splitData[1];
        System.Enum.TryParse(splitData[2], out Position position);
        SpritePosition = position;
    }
    public string CharacterName;
    [TextArea]
    public string Text;
    public Position SpritePosition;
    public bool hasCompanion;
    public Sprite CharaterImage;
}
