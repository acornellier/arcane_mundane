using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Request
{
    public ItemObject itemObject;
    public RequestImage requestImage;
}

public class MainTimer : MonoBehaviour
{
    [SerializeField] GameObject _timerUi;
    [SerializeField] TMP_Text _text;
    [SerializeField] RequestImage _requestImagePrefab;
    [SerializeField] GameObject _loseScreen;

    float _timer;
    Requester _currentRequester;
    CancellationTokenSource _cancellation;
    readonly List<Request> _activeRequests = new();

    void Awake()
    {
        _timerUi.SetActive(false);
    }

    public async UniTask RunTimer(
        CancellationToken token,
        Requester requester,
        float duration,
        int numberOfItems)
    {
        if (_currentRequester != null)
            throw new Exception("Main Timer is already active");

        _currentRequester = requester;
        requester.onRequestComplete += HandleRequestComplete;

        for (var i = 0; i < numberOfItems; ++i)
        {
            var item = requester.MakeRequest();

            _timerUi.SetActive(true);
            _timer = duration;

            var requestImage = Instantiate(_requestImagePrefab, _timerUi.transform);
            requestImage.icon.sprite = item.sprite;

            var request = new Request { itemObject = item, requestImage = requestImage, };
            _activeRequests.Add(request);
        }

        _cancellation = new CancellationTokenSource();

        await StartTimerTask(token);
    }

    void HandleRequestComplete(ItemObject itemObject)
    {
        var request =
            _activeRequests.Find(activeRequest => activeRequest.itemObject == itemObject);
        Utilities.DestroyGameObject(request.requestImage.gameObject);
        _activeRequests.Remove(request);

        if (_activeRequests.Any())
            return;

        _cancellation.Cancel();
        _timerUi.SetActive(false);
        _currentRequester.onRequestComplete -= HandleRequestComplete;
        _currentRequester = null;
    }

    async UniTask StartTimerTask(CancellationToken token)
    {
        while (_timer > 0)
        {
            if (_cancellation.IsCancellationRequested)
                return;

            _timer -= 1;
            _text.text = $"{Mathf.Ceil(_timer).ToString()}s";
            await UniTask.Delay(1000, cancellationToken: token);
        }

        _loseScreen.SetActive(true);
    }
}