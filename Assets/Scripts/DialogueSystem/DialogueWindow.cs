using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueWindow : MonoBehaviour
    {
        private int _sceneNumber = 0;
        private int _phraseNumber = 0;
        private float _waitLetter = 0.1f;
        private Dialogue _currentDialogue;
        [SerializeField] GameObject _dialogueWindow;
        [SerializeField] Text _textField;
        [SerializeField] Text _nameField;
        [SerializeField] Image _backgroundArea;
        [SerializeField] Image _leftSpritePlaceholder;
        [SerializeField] Image _rightSpritePlaceholder;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] AudioClip _typeSound;
        [SerializeField] Dialogue[] _dialogues;

        bool _isTyping;
        Coroutine _typing;

        public void StartDialogue()
        {
            _sceneNumber = 0; //get from player prefs
            _phraseNumber = 0;
            _dialogueWindow.SetActive(true);
            _backgroundArea.GetComponent<Image>().sprite = _currentDialogue.Background;
            _audioSource.clip = _currentDialogue.BackgroundMusic;
            _audioSource.Play();
            startPhrase();
        }

        void EndDialogue()
        {
            StopCoroutine(_typing);
            _isTyping = false;
            Debug.Log("end");
        }

        void startPhrase()
        {
            _typing = StartCoroutine(TypeText());
            _nameField.text = _currentDialogue.Phrases[_phraseNumber].CharacterName;
            DisplaySprites();
        }

        void DisplaySprites()
        {
            switch (_currentDialogue.Phrases[_phraseNumber].SpritePosition)
            {
                case Position.Left:
                    ShowSprite(ref _leftSpritePlaceholder);
                    if (!_currentDialogue.Phrases[_phraseNumber].HasCompanion)
                    {
                        HideSprite(ref _rightSpritePlaceholder);
                    }
                    else { DimSprite(ref _rightSpritePlaceholder); }
                    break;
                case Position.Right:
                    ShowSprite(ref _rightSpritePlaceholder);
                    if (!_currentDialogue.Phrases[_phraseNumber].HasCompanion)
                    {
                        HideSprite(ref _leftSpritePlaceholder);
                    }
                    else { DimSprite(ref _leftSpritePlaceholder); }
                    break;
                case Position.None:
                    if (!_currentDialogue.Phrases[_phraseNumber].HasCompanion)
                    {
                        HideSprite(ref _leftSpritePlaceholder);
                        HideSprite(ref _rightSpritePlaceholder);
                    }
                    else
                    {
                        DimSprite(ref _leftSpritePlaceholder);
                        DimSprite(ref _rightSpritePlaceholder);
                    }
                    break;
                default:
                    break;
            }

        }
        void HighlightSprite()
        {
            switch (_currentDialogue.Phrases[_phraseNumber].SpritePosition)
            {
                case Position.Left:
                    if (_currentDialogue.Phrases[_phraseNumber].HasCompanion)
                    {
                        _rightSpritePlaceholder.color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    _leftSpritePlaceholder.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case Position.Right:
                    if (_currentDialogue.Phrases[_phraseNumber].HasCompanion)
                    {
                        _leftSpritePlaceholder.color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    _rightSpritePlaceholder.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case Position.None:
                    if (_rightSpritePlaceholder.IsActive())
                        _rightSpritePlaceholder.color = new Color(1f, 1f, 1f, 0.5f);
                    if (_leftSpritePlaceholder.IsActive())
                        _leftSpritePlaceholder.color = new Color(1f, 1f, 1f, 0.5f);
                    break;
                default:
                    break;
            }

        }

        void ShowSprite(ref Image image)
        {
            if (!image.GetComponent<Animator>().GetBool("isShown"))
                image.GetComponent<Animator>().SetBool("isShown", true);
            image.sprite = _currentDialogue.Phrases[_phraseNumber].CharaterImage;
            image.color = new Color(1f, 1f, 1f, 1f);
        }
        void HideSprite(ref Image image)
        {
            if (image.GetComponent<Animator>().GetBool("isShown"))
                image.GetComponent<Animator>().SetBool("isShown", false);
        }
        void DimSprite(ref Image image)
        {
            if (image.GetComponent<Animator>().GetBool("isShown"))
                image.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }

        public void Skip()
        {
            if (_typing != null)
                StopCoroutine(_typing);
            _textField.text = _currentDialogue.Phrases[_phraseNumber].Text;
            _isTyping = false;
        }
        public void Next()
        {
            if (_isTyping)
            {
                Skip();
            }
            else if (_currentDialogue.Phrases.Count > _phraseNumber + 1)
            {
                _phraseNumber++;
                startPhrase();
            }
            else
            {
                EndDialogue();
            }
        }

        public void Back()
        {
            Skip();
            if (_phraseNumber > 0)
            {
                _phraseNumber--;
                startPhrase();
                Skip();
            }
        }
        public void HideText()
        {
            _dialogueWindow.GetComponent<Animator>().SetBool("isShown", false);
            HideSprite(ref _leftSpritePlaceholder);
            HideSprite(ref _rightSpritePlaceholder);
            if (_typing != null)
                StopCoroutine(_typing);
            Debug.Log("hide");

        }
        private void ShowText()
        {
            _dialogueWindow.GetComponent<Animator>().SetBool("isShown", true);
            if (_leftSpritePlaceholder.IsActive())
                _leftSpritePlaceholder.GetComponent<Animator>().SetBool("isShown", true);
            if (_rightSpritePlaceholder.IsActive())
                _rightSpritePlaceholder.GetComponent<Animator>().SetBool("isShown", true);
            Skip();
        }
        IEnumerator TypeText()
        {
            _isTyping = true;
            _textField.text = "";

            foreach (var letter in _currentDialogue.Phrases[_phraseNumber].Text)
            {
                _textField.text += letter;
                _audioSource.PlayOneShot(_typeSound);
                yield return new WaitForSeconds(_waitLetter);
            }
            _isTyping = false;
        }

        void Start()
        {
            _currentDialogue = _dialogues[_sceneNumber];
            StartDialogue();
        }

        void Update()
        {
            if (Input.GetButtonDown("Interact"))
                if (!_dialogueWindow.GetComponent<Animator>().GetBool("isShown"))
                    ShowText();
                else if (_isTyping)
                    Skip();
                else
                    Next();

        }
    }
}
