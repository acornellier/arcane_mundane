using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class MainTimer : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] Requests _humanRequests;
    [SerializeField] Requests _dragonRequests;

    [Inject] GameManager _gameManager;

    float _timer;

    void OnEnable()
    {
        _gameManager.OnGamePhaseChange += HandleGamePhaseChange;
    }

    void OnDisable()
    {
        _gameManager.OnGamePhaseChange -= HandleGamePhaseChange;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.G) && _gameManager.phase == GamePhase.Delivery)
        // _gameManager.SetPhase(GamePhase.Service);
    }

    void HandleGamePhaseChange(GamePhase oldPhase, GamePhase newPhase)
    {
        switch (newPhase)
        {
            case GamePhase.Exploring:
                _text.text = "";
                break;
            case GamePhase.Delivery:
                StartCoroutine(StartOrganizing());
                break;
            case GamePhase.Service:
                StartCoroutine(StartDelivering());
                break;
            case GamePhase.Paused:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newPhase), newPhase, null);
        }
    }

    IEnumerator StartOrganizing()
    {
        _timer = 60;
        while (_timer > 0 && _gameManager.phase == GamePhase.Delivery)
        {
            _timer -= Time.deltaTime;
            UpdateText("Delivery phase");
            yield return null;
        }

        _gameManager.SetPhase(GamePhase.Service);
    }

    IEnumerator StartDelivering()
    {
        _timer = 60;
        while (_timer > 0 && _gameManager.phase == GamePhase.Service)
        {
            _timer -= Time.deltaTime;
            UpdateText("Service phase");
            yield return null;
        }

        _gameManager.SetPhase(GamePhase.Delivery);
    }

    void UpdateText(string prefix)
    {
        _text.text = $"{prefix}: {Mathf.Ceil(_timer).ToString()}s left";
    }
}