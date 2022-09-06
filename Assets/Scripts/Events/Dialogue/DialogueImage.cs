using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueImage : MonoBehaviour
{
    [SerializeField] Image talkingHead;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text contents;

    const float _textSpeed = 50;
    const float _spriteSpeed = 10;
    const float _timeBetweenSentences = 0.5f;

    Dialogue _currentDialogue;
    Coroutine _coroutine;

    public bool IsDone()
    {
        return contents.maxVisibleCharacters >= _currentDialogue.line.Length - 1;
    }

    public void SkipToEndOfLine()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        contents.maxVisibleCharacters = _currentDialogue.line.Length;
    }

    public void TypeNextLine(Dialogue dialogue)
    {
        _coroutine = StartCoroutine(CO_TypeNextLine(dialogue));
    }

    public void StopCoroutine()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    IEnumerator CO_TypeNextLine(Dialogue dialogue)
    {
        _currentDialogue = dialogue;
        talkingHead.sprite = _currentDialogue.character.mouthClosedSprite;
        title.text = _currentDialogue.character.characterName;
        InitializeContents(_currentDialogue);

        if (_currentDialogue.wobble != Wobble.None)
        {
            contents.maxVisibleCharacters = _currentDialogue.line.Length;
            while (true)
            {
                WobbleContents(_currentDialogue.wobble);
                yield return null;
            }
        }

        var sentences = SplitIntoSentences(_currentDialogue.line);
        contents.maxVisibleCharacters = 0;
        foreach (var sentence in sentences)
        {
            var t = 0f;
            var charIndex = 0;
            while (charIndex < sentence.Length &&
                   contents.maxVisibleCharacters < _currentDialogue.line.Length)
            {
                t += Time.deltaTime;

                var newCharIndex = Mathf.Clamp(Mathf.CeilToInt(t * _textSpeed), 0, sentence.Length);
                contents.maxVisibleCharacters += newCharIndex - charIndex;
                charIndex = newCharIndex;

                talkingHead.sprite = Mathf.Floor(t * _spriteSpeed) % 2 == 0
                    ? _currentDialogue.character.mouthClosedSprite
                    : _currentDialogue.character.mouthOpenSprite;

                yield return null;
            }

            talkingHead.sprite = _currentDialogue.character.mouthClosedSprite;
            yield return new WaitForSeconds(_timeBetweenSentences);
        }
    }

    void WobbleContents(Wobble wobble)
    {
        if (wobble == Wobble.None) return;

        contents.ForceMeshUpdate();
        var mesh = contents.mesh;
        var vertices = mesh.vertices;
        var multiplier = wobble == Wobble.Slight ? 0.5f : 1f;
        for (var i = 0; i < vertices.Length; i++)
        {
            var offset = Time.time + i;
            vertices[i] += new Vector3(
                Mathf.Sin(offset * 50) * multiplier,
                Mathf.Cos(offset * 25) * multiplier
            );
        }

        mesh.vertices = vertices;
        contents.canvasRenderer.SetMesh(mesh);
    }

    void InitializeContents(Dialogue dialogue)
    {
        contents.fontSize = dialogue.fontSize switch
        {
            DialogueFontSize.Normal => 20,
            DialogueFontSize.Large => 40,
            _ => throw new ArgumentOutOfRangeException(),
        };

        contents.fontStyle = dialogue.fontStyle;
        contents.text = _currentDialogue.line;
    }

    static List<string> SplitIntoSentences(string line)
    {
        List<string> sentences = new();
        var sentence = "";
        foreach (var character in line)
        {
            if (character is not ('.' or '!' or '?'))
            {
                sentence += character;
                continue;
            }

            // account for consecutive puncutation marks
            if (sentence.Length == 0 && sentences.Count > 0)
            {
                sentences[^1] += character;
            }
            else
            {
                sentence += character;
                sentences.Add(sentence);
                sentence = "";
            }
        }

        if (sentence.Length > 0)
            sentences.Add(sentence);

        return sentences;
    }
}