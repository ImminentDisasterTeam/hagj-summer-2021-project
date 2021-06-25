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
        Coroutine _swapLeft;
        Coroutine _swapRight;
        Coroutine _hideLeft;
        Coroutine _hideRight;

        public void StartDialogue()
        {
            _rightSpritePlaceholder.sprite = null;
            _leftSpritePlaceholder.sprite = null;
            _sceneNumber = 0; //get from player prefs
            _phraseNumber = 0;
            _dialogueWindow.SetActive(true);
            _backgroundArea.GetComponent<Image>().sprite = _currentDialogue.Background;
            _audioSource.clip = _currentDialogue.BackgroundMusic;
            //_audioSource.Play();
            startPhrase();
        }

        void EndDialogue()
        {
            StopCoroutine(_typing);
            _isTyping = false;
            Debug.Log("end");
            //TODO: fadeout
        }

        void startPhrase()
        {
            StopAllCoroutines();
            _typing = StartCoroutine(TypeText());
            _nameField.text = _currentDialogue.Phrases[_phraseNumber].CharacterName;
            DisplaySprites();
        }

        void DisplaySprites()
        {
            switch (_currentDialogue.Phrases[_phraseNumber].MainSprite)
            {
                case Position.Left:
                    DisplaySprite(ref _leftSpritePlaceholder, ref _rightSpritePlaceholder, _currentDialogue.Phrases[_phraseNumber].LeftCharacter, _currentDialogue.Phrases[_phraseNumber].RightCharacter, _swapLeft, _hideLeft);
                    break;
                case Position.Right:
                    DisplaySprite(ref _rightSpritePlaceholder, ref _leftSpritePlaceholder, _currentDialogue.Phrases[_phraseNumber].RightCharacter, _currentDialogue.Phrases[_phraseNumber].LeftCharacter, _swapRight, _hideRight);
                    break;
                case Position.None:
                    _hideLeft = StartCoroutine(HideSprite(_leftSpritePlaceholder));
                    _hideRight = StartCoroutine(HideSprite(_rightSpritePlaceholder));
                    break;
                default:
                    break;
            }

        }
        void DisplaySprite(ref Image main, ref Image support, Sprite mainSprite, Sprite supportSprite, Coroutine swapCoroutine, Coroutine hideCoroutine)
        {
            if (main.sprite != null && mainSprite.name == main.sprite.name)
                HighlightSprite(ref main);
            else
                ShowSprite(ref main, mainSprite, swapCoroutine);

            if (supportSprite != null)
            {
                ShowSprite(ref support, supportSprite, swapCoroutine);
                DimSprite(ref support);
            }
            else
            {
                hideCoroutine = StartCoroutine(HideSprite(support));
            }
        }

        IEnumerator SwapSprite(Image swappingImage, Sprite sprite)
        {
            swappingImage.GetComponent<Animator>().SetBool("isShown", false);
            yield return new WaitWhile(() => swappingImage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Shown"));
            swappingImage.GetComponent<Animator>().SetBool("isShown", true);
            swappingImage.sprite = sprite;
        }
        bool showInstantly;

        void ShowSprite(ref Image image, Sprite sprite, Coroutine swapCoroutine)
        {
            if (!image.GetComponent<Animator>().GetBool("isShown") || showInstantly)
            {
                image.GetComponent<Animator>().SetBool("isShown", true);
                image.sprite = sprite;
                image.color = new Color(1f, 1f, 1f);
            }
            else if (image.sprite != null && sprite.name != image.sprite.name)
            {
                swapCoroutine = StartCoroutine(SwapSprite(image, sprite));
            }
        }
        IEnumerator HideSprite(Image image)
        {
            if (image.GetComponent<Animator>().GetBool("isShown"))
            {
                image.GetComponent<Animator>().SetBool("isShown", false);
                yield return new WaitWhile(() => image.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Shown"));
                image.sprite = null;
            }
        }
        void DimSprite(ref Image image)
        {
            if (image.GetComponent<Animator>().GetBool("isShown"))
                image.color = new Color(0.5f, 0.5f, 0.5f);
        }
        void HighlightSprite(ref Image image)
        {
            image.color = new Color(1f, 1f, 1f);
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
            showInstantly = false;
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
            showInstantly = true;
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
            _hideLeft = StartCoroutine(HideSprite(_leftSpritePlaceholder));
            _hideRight = StartCoroutine(HideSprite(_rightSpritePlaceholder));
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
