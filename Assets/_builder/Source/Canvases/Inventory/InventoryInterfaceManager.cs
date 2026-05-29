using nobodyworks.builder.extensions;
using nobodyworks.builder.inventories;
using UnityEngine;

namespace nobodyworks.builder.interfaces
{
    public class InventoryInterfaceManager : InterfaceManager
    {
        #region Inspector

        [SerializeField]
        private GameObject _slotPrefab;

        [SerializeField]
        private Transform _slotsTransform;

        #endregion

        private InventoryController _inventoryController;
        private InventorySlotWidget _hoveredSlot;

        public InventoryItem HoveredItem => _hoveredSlot?.InventoryItem;
        public bool IsHoveringSlot => _hoveredSlot != null;

        public void Setup(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
            _inventoryController.OnItemsChanged += InventoryItemsChangedHandler;

            CreateItemsSlots();
            
            OnClosed += (_) => { _hoveredSlot = null; };
        }

        private void CreateItemsSlots()
        {
            _hoveredSlot = null;
            _slotsTransform.DestroyChildren();

            foreach (var inventoryItem in _inventoryController.InventoryItems)
            {
                var slotGameObject = Instantiate(_slotPrefab, _slotsTransform);
                var slot = slotGameObject.GetComponent<InventorySlotWidget>();
                slot.Setup(inventoryItem);
                slot.OnHoverEnter += SlotHoverEnterHandler;
                slot.OnHoverExit += SlotHoverExitHandler;
            }
        }

        private void SlotHoverEnterHandler(InventorySlotWidget slot)
        {
            _hoveredSlot = slot;
        }

        private void SlotHoverExitHandler(InventorySlotWidget slot)
        {
            if (_hoveredSlot == slot)
            {
                _hoveredSlot = null;
            }
        }

        private void InventoryItemsChangedHandler()
        {
            CreateItemsSlots();
        }
    }
}
