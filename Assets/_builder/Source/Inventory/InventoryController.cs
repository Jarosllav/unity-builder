using System;
using System.Collections.Generic;
using System.Linq;
using nobodyworks.builder.items;

namespace nobodyworks.builder.inventories
{
    public class InventoryController
    {
        private readonly InventorySettings _settings;
        private readonly InventoryItem[] _inventoryItems;
        
        public IReadOnlyList<InventoryItem> InventoryItems => _inventoryItems;
        public InventorySettings Settings => _settings;
        
        public event Action OnItemsChanged;

        public InventoryController(InventorySettings settings)
        {
            _settings = settings;
            _inventoryItems = new InventoryItem[_settings.MaxCapacity];
        }
        
        public InventoryController(InventorySettings settings, ItemReference[] initialItems)
            : this(settings)
        {
            CreateItems(initialItems);
        }

        private void CreateItems(ItemReference[] itemsReferences)
        {
            for (int i = 0; i < itemsReferences.Length; ++i)
            {
                var itemReference = itemsReferences[i];
                var invItem = new InventoryItem(itemReference.Definition, itemReference.Amount);
                
                _inventoryItems[i] = invItem;
            }
        }

        public bool Add(Item item, int amount = 1)
        {
            var existingId = FindSlot(item.Definition);

            if (existingId >= 0 && item.Definition.IsStackable)
            {
                _inventoryItems[existingId].SetAmount(_inventoryItems[existingId].Amount + amount);
                OnItemsChanged?.Invoke();
                return true;
            }

            var emptyId = FindEmptySlot();

            if (emptyId < 0)
            {
                return false;
            }
            
            _inventoryItems[emptyId] = new(item, amount);
            OnItemsChanged?.Invoke();
            return true;
        }

        public bool Remove(Item item, int amount = 1)
        {
            var existingId = FindSlot(item.Definition);

            if (existingId == -1)
            {
                return false;
            }
            
            var invItem = _inventoryItems[existingId];
            
            if (invItem.Amount - amount < 0)
            {
                return false;
            }
                
            invItem.SetAmount(invItem.Amount - amount);

            if (invItem.Amount == 0)
            {
                _inventoryItems[existingId] = null;
            }
                
            OnItemsChanged?.Invoke();
            return true;
        }
        
        public bool Has(Item item)
        {
            foreach (var invItem in _inventoryItems)
            {
                if (invItem == null)
                {
                    continue;
                }
                
                if (invItem.Item.Definition == item.Definition)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        public int Count()
        {
            var count = 0;

            for (int i = 0; i < _inventoryItems.Length; ++i)
            {
                if (_inventoryItems[i] != null)
                {
                    ++count;
                }
            }
            
            return count;
        }

        private int FindSlot(ItemDefinition itemDefinition)
        {
            for (int i = 0; i < _inventoryItems.Length; ++i)
            {
                var invItem = _inventoryItems[i];

                if (invItem == null)
                {
                    continue;
                }

                if (invItem.Item.Definition == itemDefinition)
                {
                    return i;
                }
            }
            
            return -1;
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < _inventoryItems.Length; ++i)
            {
                if (_inventoryItems[i] == null)
                {
                    return i;
                }
            }
            
            return -1;
        }
    }
}