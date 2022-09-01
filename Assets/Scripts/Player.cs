using FarrokhGames.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 5;
    [SerializeField] int _inventoryWidth = 4;
    [SerializeField] int _inventoryHeight = 4;

    Rigidbody2D _body;

    public InventoryManager inventory { get; private set; }

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();

        var provider = new InventoryProvider();
        inventory = new InventoryManager(provider, _inventoryWidth, _inventoryHeight);

        inventory.onItemDropped += (item) =>
        {
            Debug.Log(((ItemObject)item).Name + " was dropped on the ground");
        };

        inventory.onItemDroppedFailed += (item) =>
        {
            Debug.Log(
                $"You're not allowed to drop {((ItemObject)item).Name} on the ground"
            );
        };

        inventory.onItemAddedFailed += (item) =>
        {
            Debug.Log($"You can't put {((ItemObject)item).Name} there!");
        };
    }

    void Update()
    {
        var inputHorizontal = Input.GetAxisRaw("Horizontal");
        var inputVertical = Input.GetAxisRaw("Vertical");

        _body.velocity = new Vector2(inputHorizontal * _speed, inputVertical * _speed);

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var item = col.GetComponent<Item>();
        if (!item) return;

        inventory.TryAdd(item._item.CreateInstance());
        Destroy(item.gameObject);
    }
}