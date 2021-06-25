using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public enum Position { None, Left, Right }

    [System.Serializable]
    public class Phrase
    {
        public Phrase(string[] data, Sprite[] sprites)
        {
            CharacterName = data[0];
            Text = data[1];
            System.Enum.TryParse(data[2], out Position position);
            MainSprite = position;
            LeftCharacter = sprites[0];
            RightCharacter = sprites[1];
        }
        public string CharacterName;
        [TextArea]
        public string Text;
        public Position MainSprite;
        public Sprite LeftCharacter;
        public Sprite RightCharacter;
    }
}
