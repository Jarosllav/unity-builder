using System;
using nobodyworks.builder.items;
using UnityEngine;

namespace nobodyworks.builder.inventories
{
    [Serializable]
    public class InventorySettings
    {
        [SerializeField, Min(1)]
        private int _maxCapacity = 1;
        
        [SerializeField]
        private ItemReference[] _startingItems;
        
        public int MaxCapacity => _maxCapacity;
        public ItemReference[] StartingItems => _startingItems;
    }
}