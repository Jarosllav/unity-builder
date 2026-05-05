using System;
using nobodyworks.builder.equipment;
using nobodyworks.builder.input;
using nobodyworks.builder.interaction;
using nobodyworks.builder.inventories;
using nobodyworks.builder.items;
using nobodyworks.builder.movement;
using nobodyworks.builder.skeleton;
using UnityEngine;
using UnityEngine.Serialization;

namespace nobodyworks.builder.character
{
    public class CharacterManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private MovementSettings _movementSettings;
        
        [SerializeField]
        private InteractionSettings _interactionSettings;
        
        [SerializeField]
        private EquipmentSettings _equipmentSettings;
        
        [SerializeField]
        private SkeletonSettings _skeletonSettings;
        
        [SerializeField]
        private ItemsDatabase _itemsDatabase; // TODO(PO): Temp
        
        #endregion
        
        private MovementController _movementController;
        private InventoryController _inventoryController;
        private InteractionController _interactionController;
        private SkeletonController _skeletonController;
        private EquipmentController _equipmentController;

        public MovementController MovementController => _movementController;
        public InventoryController InventoryController => _inventoryController;
        public InteractionController InteractionController => _interactionController;
        public SkeletonController SkeletonController => _skeletonController;
        public EquipmentController EquipmentController => _equipmentController;
        
        public void Awake()
        {
            _skeletonController = new(_skeletonSettings);
            _movementController = new(_movementSettings, _skeletonController);
            _inventoryController = new();
            _interactionController = new(_interactionSettings);
            _equipmentController = new(_inventoryController, _skeletonController, _equipmentSettings);
        }

        public void Start()
        {
            _interactionController.Register<ItemInteractableManager>((itemManager) =>
            {
                _inventoryController.Add(itemManager.GetItem());
            });
        }

        private void OnDestroy()
        {
            _movementController.Dispose();
        }

        public void Update()
        {
            var deltaTime = Time.deltaTime;
            _movementController.Tick(deltaTime);
            _interactionController.Tick(deltaTime);
        }

        #region Unity callbacks

        public void OnTriggerEnter(Collider other)
        {
            _interactionController.EnterTrigger(other);
        }

        public void OnTriggerExit(Collider other)
        {
            _interactionController.ExitTrigger(other);
        }

        #endregion

#if UNITY_EDITOR

        public void OnGUI()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            GUILayout.Label("Inventory:");
            
            for (int i = 0; i < _inventoryController.Items.Count; ++i)
            {
                var inventoryItem = _inventoryController.Items[i];
                
                GUILayout.Label($"{i + 1}. {inventoryItem.Item.Definition.Key} x{inventoryItem.Amount}");
            }

            if (_inventoryController.Items.Count <= 0)
            {
                GUILayout.Label("- empty");
            }
        }

#endif
    }
}