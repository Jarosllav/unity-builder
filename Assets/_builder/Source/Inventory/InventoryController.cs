using System;
using System.Collections.Generic;
using System.Linq;
using nobodyworks.builder.items;

namespace nobodyworks.builder.inventories
{
    public class InventoryController
    {
        private List<InventoryItem> _items = new(12);
        
        public IReadOnlyList<InventoryItem> Items => _items;
        
        public event Action OnItemsChanged;

        public InventoryController()
        {
            
        }
        
        public InventoryController(ItemReference[] initialItems)
        {
            CreateItems(initialItems);
        }

        private void CreateItems(ItemReference[] itemsReferences)
        {
            foreach (var itemReference in itemsReferences)
            {
                var item = new InventoryItem(itemReference.Definition, itemReference.Amount);
                
                _items.Add(item);
            }
        }

        public bool Add(Item item, int amount = 1)
        {
            var existing = _items.FirstOrDefault(i => i.Item.Definition == item.Definition);

            if (existing != null)
            {
                existing.SetAmount(existing.Amount + amount);
                OnItemsChanged?.Invoke();
                return true;
            }

            _items.Add(new InventoryItem(item, amount));
            OnItemsChanged?.Invoke();
            return true;
        }

        public bool Remove(Item item, int amount = 1)
        {
            var existing = _items.FirstOrDefault(i => i.Item.Definition == item.Definition);

            if (existing != null)
            {
                if (existing.Amount - amount < 0)
                {
                    return false;
                }
                
                existing.SetAmount(existing.Amount - amount);

                if (existing.Amount == 0)
                {
                    _items.Remove(existing);
                }
                
                OnItemsChanged?.Invoke();
                return true;
            }
            
            return false;
        }
        
        public bool Has(Item item)
        {
            foreach (var invItem in _items)
            {
                if (invItem.Item.Definition == item.Definition)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        public bool RemoveAtOnce(Item[] items)
        {
            var operations = new Dictionary<InventoryItem, int>();

            foreach (var item in items)
            {
                var invItem = _items
                    .Where(i => !operations.ContainsKey(i) ||  i.Amount - operations[i] > 0)
                    .FirstOrDefault(i => i.Item == item);

                if (invItem == null)
                {
                    return false;
                }

                if (!operations.TryAdd(invItem, 1))
                {
                    operations[invItem]++;
                }
            }

            foreach (var (inventoryItem, amount) in operations)
            {
                inventoryItem.SetAmount(inventoryItem.Amount - amount);

                if (inventoryItem.Amount <= 0)
                {
                    _items.Remove(inventoryItem);
                }
            }
            
            OnItemsChanged?.Invoke();
            return true;
        }
    }
}