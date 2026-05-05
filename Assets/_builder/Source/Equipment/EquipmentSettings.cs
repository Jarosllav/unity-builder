using System;
using nobodyworks.builder.items;
using UnityEngine;

namespace nobodyworks.builder.equipment
{
    [Serializable]
    public class EquipmentSettings
    {
        [SerializeField]
        private EquipmentSlotDefinition[] _slotDefinitions;
        
        [SerializeField]
        private ItemCategoryDefinition[] availableCategories;
        
        public EquipmentSlotDefinition[] SlotDefinitions => _slotDefinitions;
        public ItemCategoryDefinition[] AvailableCategories => availableCategories;
    }
}