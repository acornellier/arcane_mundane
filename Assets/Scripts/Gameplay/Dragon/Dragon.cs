using System;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
public class Dragon : NodeEventInteractable, IPersistableData
{
    [SerializeField] Animations _animations;
    [SerializeField] List<NodeEvent> _asleepNodes;

    AnimancerComponent _animancer;

    State _state = State.Awake;
    int _timesBotheredAsleep;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
    }

    void Start()
    {
        UpdateAnimations();
    }

    public void Load(PersistentData data)
    {
        if (data.dragon == null) return;

        _state = data.dragon.state;
    }

    public void Save(PersistentData data)
    {
        data.dragon = new Data { state = _state, };
    }

    public override void Interact()
    {
        if (_state == State.Asleep)
        {
            RunAsleepNode();
            return;
        }

        base.Interact();
    }

    public void SetStateAwake()
    {
        _state = State.Awake;
        _timesBotheredAsleep = 0;
        UpdateAnimations();
    }

    public void SetStateAsleep()
    {
        _state = State.Asleep;
        UpdateAnimations();
    }

    void RunAsleepNode()
    {
        var nodeEvent = _asleepNodes[_timesBotheredAsleep % _asleepNodes.Count];
        _timesBotheredAsleep += 1;
        nodeEvent.RunAndForget();
    }

    void UpdateAnimations()
    {
        _animancer.Play(_state == State.Asleep ? _animations.asleep : _animations.awake);
    }

    [Serializable]
    public enum State
    {
        Awake,
        Asleep,
    }

    [Serializable]
    class Animations
    {
        public AnimationClip awake;
        public AnimationClip asleep;
    }

    public class Data
    {
        public State state;
    }
}