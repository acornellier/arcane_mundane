using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class PhaseNodeEvent : NodeEvent
{
    [SerializeField] GamePhase phase;

    [Inject] GameManager _gameManager;

    protected override IEnumerator CO_Run()
    {
        _gameManager.SetPhase(phase);
        yield break;
    }
}