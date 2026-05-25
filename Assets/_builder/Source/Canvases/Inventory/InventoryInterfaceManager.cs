using System;
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

        public void Setup(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
            _inventoryController.OnItemsChanged += InventoryItemsChangedHandler;
            
            CreateItemsSlots();
        }
        
        private void CreateItemsSlots()
        {
            _slotsTransform.DestroyChildren();

            foreach (var inventoryItem in _inventoryController.InventoryItems)
            {
                var slotGameObject = Instantiate(_slotPrefab, _slotsTransform);
                var slotReference = slotGameObject.GetComponent<InventorySlotWidget>();
                slotReference.Setup(inventoryItem);
            }
        }

        private void InventoryItemsChangedHandler()
        {
            CreateItemsSlots();
        }
    }
}