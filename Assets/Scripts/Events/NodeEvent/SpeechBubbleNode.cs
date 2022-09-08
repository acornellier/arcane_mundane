using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SpeechBubbleNode : NodeEvent
{
    [SerializeField] TMP_Text _uiText;
    [SerializeField] string _text;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        _uiText.text = _text;
        await UniTask.Yield(token);
    }
}