using nobodyworks.builder.extensions;
using nobodyworks.builder.inventories;
using TMPro;
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
        
        [SerializeField]
        private TMP_Text _headerLabel;

        #endregion

        private InventoryController _inventoryController;
        private InventorySlotWidget _hoveredSlot;

        public InventoryItem HoveredItem => _hoveredSlot?.InventoryItem;
        public bool IsHoveringSlot => _hoveredSlot != null;

        protected override void OnStarted()
        {
            base.OnStarted();
            
            OnClosed += (_) => { _hoveredSlot = null; };
        }

        public void Setup(InventoryController inventoryController, string ownerName)
        {
            Setup(inventoryController);
            
            _headerLabel.text = ownerName;
            _headerLabel.gameObject.SetActive(true);
        }

        public void Setup(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
            _inventoryController.OnItemsChanged -= InventoryItemsChangedHandler;
            _inventoryController.OnItemsChanged += InventoryItemsChangedHandler;

            CreateItemsSlots();
            _headerLabel.gameObject.SetActive(false);
        }

        private void CreateItemsSlots()
        {
            _hoveredSlot = null;
            _slotsTransform.DestroyChildren();

            for (int i = 0; i < _inventoryController.InventoryItems.Count; ++i)
            {
                var slotGameObject = Instantiate(_slotPrefab, _slotsTransform);
                var slot = slotGameObject.GetComponent<InventorySlotWidget>();
                slot.Setup(i, this, _inventoryController.InventoryItems[i]);
                slot.OnHoverEnter += SlotHoverEnterHandler;
                slot.OnHoverExit += SlotHoverExitHandler;
                slot.OnDropped += SlotDroppedHandler;
            }
        }
        
        private void TransferSlot(InventorySlotWidget sourceSlot, InventorySlotWidget targetSlot)
        {
            var sourceInventory = sourceSlot.Owner._inventoryController;
            var targetInventory = targetSlot.Owner._inventoryController;
            
            var sourceInvItem = sourceSlot.InventoryItem;
            
            if (targetInventory.Add(sourceInvItem.Item, sourceInvItem.Amount, targetSlot.Index))
            {
                sourceInventory.Remove(sourceInvItem.Item, sourceInvItem.Amount, sourceSlot.Index);
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

        private void SlotDroppedHandler(InventorySlotWidget slot)
        {
            if (DraggableManager.Instance.Cookie is not InventorySlotWidget sourceSlot)
            {
                return;
            }

            if (sourceSlot.Owner == slot.Owner)
            {
                _inventoryController.Swap(sourceSlot.Index, slot.Index);
            }
            else
            {
                TransferSlot(sourceSlot, slot);
            }
        }
        
        private void InventoryItemsChangedHandler()
        {
            CreateItemsSlots();
        }
    }
}
