using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public enum Position { None, Left, Right }

    [System.Serializable]
    public class Phrase
    {
        public static char SplitSymbol = ';';
        public Phrase(string data)
        {
            var splitData = data.Split(SplitSymbol);
            CharacterName = splitData[0];
            Text = splitData[1];
            HasCompanion = bool.Parse(splitData[3]);
            System.Enum.TryParse(splitData[2], out Position position);
            SpritePosition = position;
        }
        public string CharacterName;
        [TextArea]
        public string Text;
        public Position SpritePosition;
        public bool HasCompanion;
        public Sprite CharaterImage;
    }
}
