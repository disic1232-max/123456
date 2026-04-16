using System;
using UnityEngine;

namespace ChemLab.Inventory
{
    /// <summary>
    /// Быстрые слоты 1-6 + расширенный инвентарь.
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private int hotbarSize = 6;
        [SerializeField] private int backpackSize = 24;

        private InventorySlot[] _hotbar;
        private InventorySlot[] _backpack;

        public int SelectedHotbarIndex { get; private set; }
        public event Action OnChanged;

        private void Awake()
        {
            _hotbar = new InventorySlot[hotbarSize];
            _backpack = new InventorySlot[backpackSize];
        }

        private void Update()
        {
            for (var i = 0; i < hotbarSize; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    SelectedHotbarIndex = i;
                    OnChanged?.Invoke();
                }
            }
        }

        public bool TryAdd(InventoryItemData item, int amount = 1)
        {
            if (item == null || amount <= 0)
                return false;

            if (item.stackable && TryAddToExistingStack(_hotbar, item, amount, out amount))
                return true;
            if (item.stackable && TryAddToExistingStack(_backpack, item, amount, out amount))
                return true;

            if (TryAddToEmptySlot(_hotbar, item, amount)) return true;
            if (TryAddToEmptySlot(_backpack, item, amount)) return true;

            return false;
        }

        public InventorySlot GetSelectedSlot() => _hotbar[SelectedHotbarIndex];

        public void SwapSlots(bool fromHotbar, int fromIndex, bool toHotbar, int toIndex)
        {
            ref var a = ref GetSlotRef(fromHotbar, fromIndex);
            ref var b = ref GetSlotRef(toHotbar, toIndex);
            (a, b) = (b, a);
            OnChanged?.Invoke();
        }

        private static bool TryAddToExistingStack(InventorySlot[] slots, InventoryItemData item, int amount, out int remaining)
        {
            remaining = amount;
            for (var i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != item || slots[i].count >= item.maxStack)
                    continue;

                var free = item.maxStack - slots[i].count;
                var move = Mathf.Min(free, remaining);
                slots[i].count += move;
                remaining -= move;

                if (remaining <= 0)
                    return true;
            }

            return false;
        }

        private bool TryAddToEmptySlot(InventorySlot[] slots, InventoryItemData item, int amount)
        {
            for (var i = 0; i < slots.Length; i++)
            {
                if (!slots[i].IsEmpty)
                    continue;

                slots[i].item = item;
                slots[i].count = Mathf.Min(amount, item.maxStack);
                OnChanged?.Invoke();
                return true;
            }

            return false;
        }

        private ref InventorySlot GetSlotRef(bool hotbar, int index)
        {
            if (hotbar)
                return ref _hotbar[index];
            return ref _backpack[index];
        }
    }
}
