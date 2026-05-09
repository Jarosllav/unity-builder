using System;
using UnityEngine;

namespace nobodyworks.builder.inventories
{
    [Serializable]
    public class InventorySettings
    {
        [SerializeField, Min(1)]
        private int _maxCapacity = 1;
        
        public int MaxCapacity => _maxCapacity;
    }
}