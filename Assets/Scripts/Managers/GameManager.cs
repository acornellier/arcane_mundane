using System;
using UnityEngine;
using Zenject;

public enum GamePhase
{
    Exploring,
    Delivery,
    Service,
    Paused,
}

public class GameManager : IInitializable
{
    public GamePhase phase { get; private set; } = GamePhase.Exploring;
    public event Action<GamePhase, GamePhase> OnGamePhaseChange;

    public bool isPaused { get; private set; }
    public event Action<bool> OnGamePausedChange;

    public void Initialize()
    {
        Time.timeScale = 1;
    }

    public void SetPhase(GamePhase newPhase)
    {
        if (phase == newPhase) return;

        var oldState = phase;

        phase = newPhase;

        OnGamePhaseChange?.Invoke(oldState, newPhase);
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;

        if (isPaused)
            Time.timeScale = 0;
        else if (isPaused)
            Time.timeScale = 1;

        OnGamePausedChange?.Invoke(isPaused);
    }
}