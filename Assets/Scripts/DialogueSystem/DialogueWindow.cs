using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace DialogueSystem
{
    public class DialogueWindow : MonoBehaviour
    {
        private int _dialogueSceneNumber = 0;
        private int _phraseNumber = 0;
        private float _waitLetter = 0.1f;
        private Dialogue _currentDialogue;
        private Animator _windowAnimator;
        private Animator _leftCharacterAnimator;
        private Animator _rightCharacterAnimator;
        public Action NextScene;
        [SerializeField] SoundManager _soundManager;
        [SerializeField] GameObject _dialogueWindow;
        [SerializeField] Text _textField;
        [SerializeField] Text _nameField;
        [SerializeField] Image _backgroundArea;
        [SerializeField] Image _leftSpritePlaceholder;
        [SerializeField] Image _rightSpritePlaceholder;
        [SerializeField] AudioClip _typeSound;
        [SerializeField] Dialogue[] _dialogues;

        bool _isTyping;
        Coroutine _typing;

        public void StartDialogue()
        {
            _rightSpritePlaceholder.sprite = null;
            _leftSpritePlaceholder.sprite = null;
            _dialogueSceneNumber = PlayerPrefs.GetInt("dialogue");
            _currentDialogue = _dialogues[_dialogueSceneNumber];
            _phraseNumber = 0;
            _dialogueWindow.SetActive(true);
            _backgroundArea.GetComponent<Image>().sprite = _currentDialogue.Background;
            _soundManager.PlayMusic(_currentDialogue.BackgroundMusic);
            startPhrase();
        }

        void EndDialogue()
        {
            NextScene?.Invoke();
        }
        void startPhrase()
        {
            if(_leftCharacterAnimator.GetBool("isShown"))
                _leftCharacterAnimator.SetBool("isShown",false);
            if (_rightCharacterAnimator.GetBool("isShown"))
                _rightCharacterAnimator.SetBool("isShown", false);
            StopAllCoroutines();
            _typing = StartCoroutine(TypeText());
            _nameField.text = _currentDialogue.Phrases[_phraseNumber].CharacterName;
            DisplaySprites(_leftSpritePlaceholder, _rightSpritePlaceholder, _currentDialogue.Phrases[_phraseNumber]);
        }
        IEnumerator TypeText()
        {
            _isTyping = true;
            _textField.text = "";

            foreach (var letter in _currentDialogue.Phrases[_phraseNumber].Text)
            {
                _textField.text += letter;
                _soundManager.PlaySound(_typeSound);
                yield return new WaitForSeconds(_waitLetter);
            }
            _isTyping = false;
        }

        void DisplaySprites(Image leftImage, Image rightImage, Phrase phrase, bool showInstantly = false)
        {
            switch (phrase.MainSprite)
            {
                case Position.Left:
                    DisplaySpriteCorrectly(leftImage, rightImage, phrase.LeftCharacter, phrase.RightCharacter, showInstantly);
                    break;
                case Position.Right:
                    DisplaySpriteCorrectly(rightImage, leftImage, phrase.RightCharacter, phrase.LeftCharacter, showInstantly);
                    break;
                case Position.None:
                    if (leftImage.sprite != null)
                        StartCoroutine(ChangeSprite(leftImage));
                    if (rightImage.sprite != null)
                        StartCoroutine(ChangeSprite(rightImage));
                    break;
                default:
                    break;
            }

        }
        void DisplaySpriteCorrectly(Image main, Image support, Sprite mainSprite, Sprite supportSprite, bool showInstantly = false)
        {
            if (main.sprite != null && mainSprite.name == main.sprite.name)
            {
                ChangeSpriteColor(main, false);
            }
            else
            {
                ShowSprite(main, mainSprite);
            }

            if (supportSprite != null)
            {
                if (support.sprite != null && mainSprite.name == main.sprite.name)
                    ChangeSpriteColor(support, true);
                else
                    ShowSprite(support, supportSprite);
            }
            else
            {
                StartCoroutine(ChangeSprite(support));
            }
        }
        void ShowSprite(Image image, Sprite sprite, bool showInstantly = false)
        {
            Animator animator = image.GetComponent<Animator>();
            if (!animator.GetBool("isShown") || showInstantly)
            {
                animator.SetBool("isShown", true);
                image.sprite = sprite;
            }
            else if (image.sprite != null && sprite.name != image.sprite.name)
            {
                StartCoroutine(ChangeSprite(image, sprite));
            }
        }
        IEnumerator ChangeSprite(Image image, Sprite sprite = null)
        {
            if (image.GetComponent<Animator>().GetBool("isShown"))
                image.GetComponent<Animator>().SetBool("isShown", false);
            yield return new WaitWhile(() => image.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Shown"));
            if (sprite == null)
            {
                image.sprite = sprite;
            }
            else
            {
                image.GetComponent<Animator>().SetBool("isShown", true);
                image.sprite = sprite;

            }
        }
        void ChangeSpriteColor(Image image, bool dim = false)
        {
            if (dim)
            {
                if (image.GetComponent<Animator>().GetBool("isShown"))
                {
                    image.color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
            else
            {
                image.color = new Color(1f, 1f, 1f);
            }
        }
        void ShowText()
        {
            _windowAnimator.SetBool("isShown", true);
            if (_leftSpritePlaceholder.sprite != null)
                _leftCharacterAnimator.SetBool("isShown", true);
            if (_rightSpritePlaceholder.sprite != null)
                _rightCharacterAnimator.SetBool("isShown", true);
            Skip();
        }
        public void HideText()
        {
            _dialogueWindow.GetComponent<Animator>().SetBool("isShown", false);
            _leftCharacterAnimator.SetBool("isShown", false);
            _rightCharacterAnimator.SetBool("isShown", false);
            if (_typing != null)
                StopCoroutine(_typing);
        }
        public void Back()
        {
            //StopAllCoroutines();
            if (_phraseNumber > 0)
            {
                _phraseNumber--;
                startPhrase();
                Skip();
            }
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
        public void Skip()
        {
            if (_typing != null)
                StopCoroutine(_typing);
            _textField.text = _currentDialogue.Phrases[_phraseNumber].Text;
            _isTyping = false;
        }

        void Awake()
        {
            _windowAnimator = _dialogueWindow.GetComponent<Animator>();
            _leftCharacterAnimator = _leftSpritePlaceholder.GetComponent<Animator>();
            _rightCharacterAnimator = _rightSpritePlaceholder.GetComponent<Animator>();
            // StartDialogue();
        }

        void Update()
        {
            if (!PauseController.Paused && Input.GetButtonDown("Interact"))
                if (!_windowAnimator.GetBool("isShown"))
                    ShowText();
                else if (_isTyping)
                    Skip();
                else
                    Next();

        }
    }
}
