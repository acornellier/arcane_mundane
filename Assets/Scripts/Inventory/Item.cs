using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    public ItemObject item;

    void OnValidate()
    {
        if (item)
            gameObject.GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
}