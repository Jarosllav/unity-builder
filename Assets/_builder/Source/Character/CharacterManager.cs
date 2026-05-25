using System;
using System.Text;
using nobodyworks.builder.carrying;
using nobodyworks.builder.equipment;
using nobodyworks.builder.extensions;
using nobodyworks.builder.input;
using nobodyworks.builder.interaction;
using nobodyworks.builder.inventories;
using nobodyworks.builder.items;
using nobodyworks.builder.movement;
using nobodyworks.builder.placement;
using nobodyworks.builder.skeleton;
using UnityEngine;

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
        private InventorySettings _inventorySettings;
        
        [SerializeField]
        private EquipmentSettings _equipmentSettings;
        
        [SerializeField]
        private SkeletonSettings _skeletonSettings;
        
        [SerializeField]
        private CarrierSettings _carrierSettings;
        
        [SerializeField]
        private PlacementSettings _placementSettings;
        
        #endregion
        
        private MovementController _movementController;
        private InventoryController _inventoryController;
        private InteractionController _interactionController;
        private SkeletonController _skeletonController;
        private EquipmentController _equipmentController;
        private CarrierController _carrierController;
        private PlacementController _placementController;
        
        private bool _isInstalled = false;
        
        public MovementController MovementController => _movementController;
        public InventoryController InventoryController => _inventoryController;
        public InteractionController InteractionController => _interactionController;
        public SkeletonController SkeletonController => _skeletonController;
        public EquipmentController EquipmentController => _equipmentController;
        public CarrierController CarrierController => _carrierController;
        public PlacementController PlacementController => _placementController;
        public bool IsInstalled => _isInstalled;
        
        public event Action OnInstalled;

        public void Install()
        {
            _skeletonController = new(_skeletonSettings);
            _movementController = new(_movementSettings, _skeletonController);
            _inventoryController = new(_inventorySettings);
            _interactionController = new(_interactionSettings);
            _equipmentController = new(_inventoryController, _skeletonController, _equipmentSettings);
            _carrierController = new(_carrierSettings, _equipmentController, _movementController);
            _placementController = new(_placementSettings, _movementController);

            CreateEvents();
            
            _isInstalled = true;
            OnInstalled?.Invoke();
        }
        
        private void CreateEvents()
        {
            _interactionController.Register<ItemInteractableManager>((itemManager, _) =>
            {
                _inventoryController.Add(itemManager.GetItem());
            });
            
            _interactionController.Register<ICarryable>((carryable, interactionType) =>
            {
                if (interactionType == InteractionType.Secondary)
                {
                    _carrierController.Take(carryable);
                }
            });

            _interactionController.Register<CrateInteractableManager>((crateManager, interactionType) =>
            {
                if (interactionType != InteractionType.Primary)
                {
                    return;
                }
            });
            
            _carrierController.OnCarryStarted += () =>
            {
                if (_carrierController.Carrying is IPlaceable placeable)
                {
                    _placementController.Create(placeable);
                }
            };
            
            _carrierController.OnCarryEnded += () =>
            {
                if (_carrierController.Carrying is IPlaceable)
                {
                    _placementController.Destroy();
                }
            };
            
            _carrierController.EndCondition.Subscribe(() =>
            {
                if (_carrierController.Carrying is IPlaceable)
                {
                    return _placementController.CanPlace();
                }
                
                return true;
            });
        }

        public void OnDestroy()
        {
            _movementController.Dispose();
            _interactionController.Dispose();
            _equipmentController.Dispose();
            
            OnInstalled = null;
        }

        public void Update()
        {
            if (!_isInstalled)
            {
                return;
            }
            
            var deltaTime = Time.deltaTime;
            
            _movementController.Tick(deltaTime);
            _interactionController.Tick(deltaTime);
            _carrierController.Tick(deltaTime);
            _placementController.Tick(deltaTime);
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
        }

#endif
    }
}