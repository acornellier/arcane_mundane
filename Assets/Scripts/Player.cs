using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] InventoryObject inventory;
    [SerializeField] float speed = 5;

    Rigidbody2D _body;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var inputHorizontal = Input.GetAxisRaw("Horizontal");
        var inputVertical = Input.GetAxisRaw("Vertical");

        _body.MovePosition(
            (Vector2)transform.position +
            speed * Time.deltaTime * new Vector2(inputHorizontal, inputVertical)
        );
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var item = col.GetComponent<Item>();
        if (!item) return;

        inventory.AddItem(item.item);
        Destroy(item.gameObject);
    }

    void OnApplicationQuit()
    {
        inventory.container.Clear();
    }
}