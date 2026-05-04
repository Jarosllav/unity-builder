using System;
using nobodyworks.builder.input;
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
        private ItemsDatabase _itemsDatabase; // TODO(PO): Temp
        
        #endregion
        
        private MovementController _movementController;
        private InventoryController _inventoryController;

        public MovementController MovementController => _movementController;
        public InventoryController InventoryController => _inventoryController;
        
        public void Awake()
        {
            var movementDriver = new CharacterControllerDriver(_characterController);
            
            _movementController = new(movementDriver, transform, _eyesTransform, _eyesBoneTransform, 
                _feetBoneTransform, _groundMask, _groundDistanceCheck, _movementStates);
            
            _inventoryController = new();
        }

        public void Start()
        {
            _inventoryController.Add(new Item(_itemsDatabase.GetDefinition("hammer")));

            foreach (var invItem in _inventoryController.Items)
            {
                Debug.Log($"- {invItem.Item.Definition.Key} x{invItem.Amount}");
            }
        }

        private void OnDestroy()
        {
            _movementController.Dispose();
        }

        public void Update()
        {
            var deltaTime = Time.deltaTime;
            _movementController.Tick(deltaTime);
        }
    }
}