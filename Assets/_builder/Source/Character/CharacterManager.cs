using System;
using nobodyworks.builder.input;
using nobodyworks.builder.interaction;
using nobodyworks.builder.inventories;
using nobodyworks.builder.items;
using nobodyworks.builder.movement;
using UnityEngine;
using UnityEngine.Serialization;

namespace nobodyworks.builder.character
{
    public class CharacterManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private CharacterController _characterController;

        [SerializeField]
        private Transform _eyesTransform;
        
        [SerializeField]
        private Transform _eyesBoneTransform;
        
        [SerializeField]
        private Transform _feetBoneTransform;
        
        [SerializeField]
        private LayerMask _groundMask;
        
        [SerializeField]
        private float _groundDistanceCheck;
        
        [SerializeField]
        private MovementState[] _movementStates;
        
        [SerializeField]
        private LayerMask _interactionMask;
        
        [SerializeField]
        private ItemsDatabase _itemsDatabase; // TODO(PO): Temp
        
        #endregion
        
        private MovementController _movementController;
        private InventoryController _inventoryController;
        private InteractionController _interactionController;

        public MovementController MovementController => _movementController;
        public InventoryController InventoryController => _inventoryController;
        public InteractionController InteractionController => _interactionController;
        
        public void Awake()
        {
            var movementDriver = new CharacterControllerDriver(_characterController);
            
            _movementController = new(movementDriver, transform, _eyesTransform, _eyesBoneTransform, 
                _feetBoneTransform, _groundMask, _groundDistanceCheck, _movementStates);
            _inventoryController = new();
            _interactionController = new(_interactionMask, _eyesTransform);
        }

        public void Start()
        {
            // _inventoryController.Add(new Item(_itemsDatabase.GetDefinition("hammer")));

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