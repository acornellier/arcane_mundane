using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    public ItemObject _item;

    void OnValidate()
    {
        if (_item)
            gameObject.GetComponent<SpriteRenderer>().sprite = _item.sprite;
    }
}