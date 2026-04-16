using UnityEngine;

namespace ChemLab.Inventory
{
    [CreateAssetMenu(menuName = "ChemLab/Inventory/Item", fileName = "Item_")]
    public class InventoryItemData : ScriptableObject
    {
        public string itemId;
        public string displayName;
        public Sprite icon;
        public GameObject worldPrefab;
        public bool stackable;
        public int maxStack = 1;
    }
}
