using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class SpeechBubbleNodeEvent : NodeEvent
{
    [SerializeField] TMP_Text _uiText;
    [SerializeField] string _text;

    protected override IEnumerator CO_Run()
    {
        _uiText.text = _text;
        yield break;
    }
}