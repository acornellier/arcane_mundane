using UnityEngine;
using UnityEngine.UI;

namespace FarrokhGames.Inventory.Examples
{
    public class InventorySelection : MonoBehaviour
    {
        Text _text;

        void Start()
        {
            _text = GetComponentInChildren<Text>();
            _text.text = string.Empty;

            var allControllers = FindObjectsOfType<InventoryController>();

            foreach (var controller in allControllers)
            {
                controller.onItemHovered += HandleItemHover;
            }
        }

        void HandleItemHover(IInventoryItem item)
        {
            _text.text = item != null ? ((ItemDefinition)item).Name : string.Empty;
        }
    }
}