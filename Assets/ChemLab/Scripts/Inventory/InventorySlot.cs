namespace ChemLab.Inventory
{
    [System.Serializable]
    public struct InventorySlot
    {
        public InventoryItemData item;
        public int count;

        public bool IsEmpty => item == null || count <= 0;

        public void Clear()
        {
            item = null;
            count = 0;
        }
    }
}
