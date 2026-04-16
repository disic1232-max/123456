using ChemLab.Inventory;
using UnityEngine;

namespace ChemLab.Core
{
    /// <summary>
    /// ПКМ — подобрать, ЛКМ — использовать/поставить.
    /// </summary>
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactDistance = 3f;
        [SerializeField] private LayerMask interactMask;
        [SerializeField] private PlayerInventory inventory;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) // ПКМ
                TryPickup();

            if (Input.GetMouseButtonDown(0)) // ЛКМ
                TryUseOrPlace();
        }

        private void TryPickup()
        {
            if (!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                    out var hit, interactDistance, interactMask))
                return;

            var worldItem = hit.collider.GetComponent<WorldInventoryItem>();
            if (worldItem == null)
                return;

            if (inventory.TryAdd(worldItem.ItemData, worldItem.Amount))
                Destroy(worldItem.gameObject);
        }

        private void TryUseOrPlace()
        {
            var selected = inventory.GetSelectedSlot();
            if (selected.IsEmpty)
                return;

            if (!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                    out var hit, interactDistance, interactMask))
                return;

            var spawnPos = hit.point + hit.normal * 0.1f;
            Instantiate(selected.item.worldPrefab, spawnPos, Quaternion.identity);
        }
    }
}
