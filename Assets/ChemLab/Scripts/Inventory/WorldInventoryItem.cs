using UnityEngine;

namespace ChemLab.Inventory
{
    public class WorldInventoryItem : MonoBehaviour
    {
        [field: SerializeField] public InventoryItemData ItemData { get; private set; }
        [field: SerializeField] public int Amount { get; private set; } = 1;
    }
}
