using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using nobodyworks.builder.items;
using nobodyworks.builder.inventories;
using nobodyworks.builder.skeleton;

namespace nobodyworks.builder.equipment
{
    public class EquipmentController : IDisposable
    {
        private readonly InventoryController _inventoryController;
        private readonly SkeletonController _skeletonController;
        private readonly EquipmentSettings _settings;

        private EquipmentSlotController[] _slotsControllers;

        public IReadOnlyList<EquipmentSlotController> SlotsControllers => _slotsControllers;

        public event Action<Item> OnItemEquipped;
        public event Action<Item> OnItemUnequipped;

        #region Initialization

        public EquipmentController(InventoryController inventoryController, SkeletonController skeletonController,
            EquipmentSettings settings)
        {
            _inventoryController = inventoryController;
            _skeletonController = skeletonController;
            _settings = settings;
            
            CreateSlotsControllers();
        }

        public void Dispose()
        {
            OnItemEquipped = null;
            OnItemUnequipped = null;
        }

        private void CreateSlotsControllers()
        {
            _slotsControllers = new EquipmentSlotController[_settings.SlotDefinitions.Length];

            for (int i = 0; i < _slotsControllers.Length; ++i)
            {
                var slotDefinition = _settings.SlotDefinitions[i];
                var boneReference = _skeletonController.GetBone(slotDefinition.BoneDefinition);

                _slotsControllers[i] = new EquipmentSlotController(slotDefinition, boneReference);
            }
        }

        #endregion

        public bool Equip(Item item)
        {
            var itemDefinition = item.Definition;

            if (itemDefinition == null || !itemDefinition.IsEquippable)
            {
                return false;
            }

            if (!_inventoryController.Has(item))
            {
                return false;
            }

            var slotDefinition = item.Definition.SlotDefinition;
            var slotController = GetSlotController(slotDefinition);

            if (slotController == null)
            {
                return false;
            }

            if (slotController.Item != null)
            {
                Unequip(slotController.Item);
            }

            if (!slotController.Equip(item))
            {
                return false;
            }

            OnItemEquipped?.Invoke(item);
            return true;
        }

        public bool Unequip(Item item, bool detachFromBone = true)
        {
            var itemDefinition = item.Definition;

            if (itemDefinition == null || !itemDefinition.IsEquippable)
            {
                return false;
            }

            var slotDefinition = item.Definition.SlotDefinition;
            var slotController = GetSlotController(slotDefinition);

            if (slotController == null || !slotController.Unequip(item))
            {
                return false;
            }

            OnItemUnequipped?.Invoke(item);
            return true;
        }

        public bool IsEquipped(Item item)
        {
            return IsEquipped(item.Definition);
        }
        
        public bool IsEquipped(ItemDefinition itemDefinition)
        {
            if (itemDefinition == null)
            {
                return false;
            }
            
            if (!itemDefinition.IsEquippable)
            {
                return false;
            }

            var slotDefinition = itemDefinition.SlotDefinition;
            var slotController = GetSlotController(slotDefinition);

            if (slotController == null || slotController.Item == null)
            {
                return false;
            }

            return slotController.Item.Definition == itemDefinition;
        }
        
        public EquipmentSlotController GetSlotController(EquipmentSlotDefinition slotDefinition)
        {
            foreach (var slotController in _slotsControllers)
            {
                if (slotController.Definition == slotDefinition)
                {
                    return slotController;
                }
            }

            return null;
        }
    }
}
