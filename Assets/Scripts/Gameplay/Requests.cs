using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class Requests : MonoBehaviour
{
    [SerializeField] Vector2Int _waitTime;
    [SerializeField] Image _wrapperImage;
    [SerializeField] Image _requestImage;
    [SerializeField] TMP_Text _text;

    [Inject] GameManager _gameManager;

    Coroutine _requestCoroutine;
    ItemObject _currentRequest;
    float _timer;

    void OnEnable()
    {
        _gameManager.OnGamePhaseChange += HandleGamePhaseChange;
    }

    void OnDisable()
    {
        _gameManager.OnGamePhaseChange -= HandleGamePhaseChange;
    }

    void Start()
    {
        EndRequest();
    }

    void Update()
    {
        if (!_currentRequest) return;

        _timer -= Time.deltaTime;

        if (_timer <= 0)
            NewRequest();

        _text.text = Mathf.Ceil(_timer).ToString();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (!player) return;

        var topOfStack = player.topOfStack;
        if (!topOfStack || topOfStack.itemObject != _currentRequest)
            return;

        player.DestroyTop();
        NewRequest();
    }

    void HandleGamePhaseChange(GamePhase oldPhase, GamePhase newPhase)
    {
        if (newPhase == GamePhase.Service)
        {
            NewRequest();
            return;
        }

        if (oldPhase == GamePhase.Service)
            EndRequest();
    }

    void NewRequest()
    {
        _requestCoroutine = StartCoroutine(CO_NewRequest());
    }

    void EndRequest()
    {
        if (_requestCoroutine != null)
            StopCoroutine(_requestCoroutine);

        _currentRequest = null;
        _wrapperImage.gameObject.SetActive(false);
    }

    IEnumerator CO_NewRequest()
    {
        var prevItem = _currentRequest;
        EndRequest();

        var items = FindObjectsOfType<Item>();
        if (items.Length == 0)
            yield break;

        while (_currentRequest == null || _currentRequest == prevItem)
        {
            _currentRequest = items[Random.Range(0, items.Length)].itemObject;
        }

        _timer = Random.Range(_waitTime.x, _waitTime.y);
        _wrapperImage.gameObject.SetActive(true);
        _requestImage.sprite = _currentRequest.sprite;
    }
}