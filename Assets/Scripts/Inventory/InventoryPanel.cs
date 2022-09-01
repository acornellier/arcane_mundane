using FarrokhGames.Inventory;
using UnityEngine;

[RequireComponent(typeof(InventoryRenderer))]
public class InventoryPanel : MonoBehaviour
{
    void Start()
    {
        var player = FindObjectOfType<Player>();
        GetComponent<InventoryRenderer>().SetInventory(player.inventory);
    }
}