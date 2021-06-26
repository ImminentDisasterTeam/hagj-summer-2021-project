using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class Dialogue : ScriptableObject
    {
        public string Title;
        public List<Phrase> Phrases;
        public Sprite Background;
        public AudioClip BackgroundMusic;
        public int Episode;
    }
}