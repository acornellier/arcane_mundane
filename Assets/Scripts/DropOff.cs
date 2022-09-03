using UnityEngine;
using UnityEngine.UI;

public class DropOff : MonoBehaviour
{
    [SerializeField] ItemObjectList _allitems;
    [SerializeField] Image _requestImage;

    ItemObject _currentRequest;

    void Start()
    {
        NewRequest();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (!player) return;

        var topOfStack = player.topOfStack;
        if (!topOfStack || topOfStack.item != _currentRequest)
        {
            print("no");
            return;
        }

        player.DestroyTop();
        NewRequest();
    }

    void NewRequest()
    {
        _currentRequest = _allitems.items[Random.Range(0, _allitems.items.Length)];
        _requestImage.sprite = _currentRequest.sprite;
    }
}