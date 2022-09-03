using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Requests : MonoBehaviour
{
    [SerializeField] Image _wrapperImage;
    [SerializeField] Image _requestImage;

    ItemObject _currentRequest;

    void Start()
    {
        StartCoroutine(CO_NewRequest());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (!player) return;

        var topOfStack = player.topOfStack;
        if (!topOfStack || topOfStack.item != _currentRequest)
            return;

        player.DestroyTop();
        StartCoroutine(CO_NewRequest(topOfStack.item));
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    IEnumerator CO_NewRequest(ItemObject prevItem = null)
    {
        _wrapperImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(Random.Range(3, 7));

        var items = FindObjectsOfType<Item>();

        while (_currentRequest == prevItem)
        {
            _currentRequest = items[Random.Range(0, items.Length)].item;
        }

        _wrapperImage.gameObject.SetActive(true);
        _requestImage.sprite = _currentRequest.sprite;
    }
}