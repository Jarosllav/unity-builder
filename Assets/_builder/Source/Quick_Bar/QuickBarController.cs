using System;
using nobodyworks.builder.inventories;
using nobodyworks.builder.items;

namespace nobodyworks.builder.quickbar
{
    public class QuickBarController
    {
        private readonly QuickBarSettings _settings;
        private readonly InventoryController _inventoryController;
        private readonly ItemDefinition[] _slots;

        public int Capacity => _settings.Capacity;

        public event Action<int, ItemDefinition> OnSlotChanged;

        #region Initialization

        public QuickBarController(QuickBarSettings settings, InventoryController inventoryController)
        {
            _settings = settings;
            _inventoryController = inventoryController;
            _slots = new ItemDefinition[_settings.Capacity];
            
            _inventoryController.OnItemsChanged += () =>
            {
                for (int i = 0; i < _slots.Length; i++)
                {
                    var itemDefinition = _slots[i];
                    
                    if (!_inventoryController.Has(itemDefinition))
                    {
                        Clear(i);
                    }
                }
            };
        }

        public void Dispose()
        {
            OnSlotChanged = null;
        }

        #endregion

        public void Assign(int slotIndex, ItemDefinition itemDefinition)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Length)
            {
                return;
            }

            _slots[slotIndex] = itemDefinition;
            OnSlotChanged?.Invoke(slotIndex, itemDefinition);
        }

        public void Assign(ItemDefinition itemDefinition)
        {
            Assign(GetFreeSlotIndex(), itemDefinition);
        }

        public void Clear(int slotIndex)
        {
            Assign(slotIndex, null);
        }

        public ItemDefinition GetSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _slots.Length)
            {
                return null;
            }

            return _slots[slotIndex];
        }

        private int GetFreeSlotIndex()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == null)
                {
                    return i;
                }
            }
            
            return -1;
        }
    }
}
