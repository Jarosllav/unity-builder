using System;
using nobodyworks.builder.equipment;
using UnityEngine;

namespace nobodyworks.builder.carrying
{
    [Serializable]
    public class CarrierSettings
    {
        [SerializeField]
        private EquipmentSlotDefinition _primarySlotDefinition;
        
        [SerializeField]
        private EquipmentSlotDefinition _secondarySlotDefinition;
        
        [SerializeField]
        private EquipmentSlotDefinition _carrySlotDefinition;
        
        [SerializeField]
        private Material _greenMaterial;
        
        [SerializeField]
        private Material _redMaterial;
        
        public EquipmentSlotDefinition PrimarySlotDefinition => _primarySlotDefinition;
        public EquipmentSlotDefinition SecondarySlotDefinition => _secondarySlotDefinition;
        public EquipmentSlotDefinition CarrySlotDefinition => _carrySlotDefinition;
        public Material GreenMaterial => _greenMaterial;
        public Material RedMaterial => _redMaterial;
    }
}