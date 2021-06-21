using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        StopAllCoroutines();
        _isTyping = false;
        Debug.Log("end");
    }

    void startPhrase()
    {
        StartCoroutine(TypeText());
        _nameField.text = _currentDialogue.Phrases[_phraseNumber].CharacterName;

        switch (_currentDialogue.Phrases[_phraseNumber].SpritePosition)
        {
            case Position.Left:
                _leftSpritePlaceholder.gameObject.SetActive(true);
                _leftSpritePlaceholder.sprite = _currentDialogue.Phrases[_phraseNumber].CharaterImage;
                break;
            case Position.Right:
                _rightSpritePlaceholder.gameObject.SetActive(true);
                _rightSpritePlaceholder.sprite = _currentDialogue.Phrases[_phraseNumber].CharaterImage;
                break;
            default:
                break;
        }
        HighlightSprite();
    }

    public void Skip()
    {
        StopAllCoroutines();
        _textField.text = _currentDialogue.Phrases[_phraseNumber].Text;
        _isTyping = false;
    }
    public void Next()
    {
        if (_currentDialogue.Phrases.Count > _phraseNumber + 1)
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
        if (_phraseNumber > 0)
        {
            _phraseNumber--;
            startPhrase();
            Skip();
        }
    }
    public void HideText()
    {
        _dialogueWindow.SetActive(false);
        Debug.Log("hide");

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
    void HighlightSprite()
    {
        switch (_currentDialogue.Phrases[_phraseNumber].SpritePosition)
        {
            case Position.Left:
                if (_rightSpritePlaceholder != null)
                    _rightSpritePlaceholder.color = new Color(1f, 1f, 1f, 0.5f);
                _leftSpritePlaceholder.color = new Color(1f, 1f, 1f, 1f);
                break;
            case Position.Right:
                if (_leftSpritePlaceholder != null)
                    _leftSpritePlaceholder.color = new Color(1f, 1f, 1f, 0.5f);
                _rightSpritePlaceholder.color = new Color(1f, 1f, 1f, 1f);
                break;
            case Position.None:
                if (_rightSpritePlaceholder != null)
                    _rightSpritePlaceholder.color = new Color(1f, 1f, 1f, 0.5f);
                if (_leftSpritePlaceholder != null)
                    _leftSpritePlaceholder.color = new Color(1f, 1f, 1f, 0.5f);
                break;
            default:
                break;
        }

    }

    void Start()
    {
        _currentDialogue = _dialogues[_sceneNumber];
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
            if (!_dialogueWindow.activeSelf)
                _dialogueWindow.SetActive(true);
            else if (_isTyping)
                Skip();
            else
                Next();

    }
}
