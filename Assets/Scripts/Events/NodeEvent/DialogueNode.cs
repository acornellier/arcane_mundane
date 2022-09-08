using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class DialogueNode : NodeEvent
{
    [SerializeField] Dialogue[] dialogues;

    [Inject] DialogueManager _dialogueManager;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        _dialogueManager.StartDialogue(dialogues);
        await UniTask.WaitWhile(() => _dialogueManager.isActive, cancellationToken: token);
    }
}