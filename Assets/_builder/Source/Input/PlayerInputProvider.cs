using System;
using nobodyworks.builder.character;
using nobodyworks.builder.interaction;
using nobodyworks.builder.movement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace nobodyworks.builder.input
{
    public class PlayerInputProvider : MonoBehaviour, IInputProvider
    {
        #region Inspector

        [SerializeField]
        private CharacterController _characterController;

        #endregion
        
        private InputSystem_Actions _actionAsset;
        private CharacterManager _characterManager;
        private MovementController _movementController;
        
        public void Awake()
        {
            _actionAsset = new();
            _characterManager = GetComponent<CharacterManager>();
            
            Assert.IsNotNull(_characterManager);
            
            _actionAsset.Player.Interact_Primary.performed += (ctx) => Interact(InteractionType.Primary);
            _actionAsset.Player.Interact_Secondary.performed += (ctx) => Interact(InteractionType.Secondary);
            
            _actionAsset.Player.Quick_1.performed += (ctx) => Quick(0);
            _actionAsset.Player.Quick_2.performed += (ctx) => Quick(1);
            _actionAsset.Player.Quick_3.performed += (ctx) => Quick(2);
            _actionAsset.Player.Quick_4.performed += (ctx) => Quick(3);
        }

        public void Start()
        {
            var movementDriver = new CharacterControllerDriver(_characterController);

            _movementController = _characterManager.MovementController;
            _movementController.SetDriver(movementDriver);
            
            _actionAsset.Enable();
            
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Update()
        {
            if (_movementController == null)
            {
                return;
            }
            
            _movementController.Move(GetMove());
            _movementController.RotateDelta(GetLook());
        }

        public Vector2 GetMove()
        {
            return _actionAsset.Player.Move.ReadValue<Vector2>();
        }
        
        public Vector2 GetLook()
        {
            return _actionAsset.Player.Look.ReadValue<Vector2>();
        }

        private void Interact(InteractionType interactionType)
        {
            if (interactionType == InteractionType.Primary && _characterManager.CarrierController.IsCarrying)
            {
                _characterManager.CarrierController.Drop();
                return;
            }
            
            _characterManager.InteractionController.Use(interactionType);
        }

        private void Quick(int id)
        {
            var invItem = _characterManager.InventoryController.Items[id];

            if (_characterManager.EquipmentController.IsEquipped(invItem.Item))
            {
                _characterManager.EquipmentController.Unequip(invItem.Item);
            }
            else
            {
                _characterManager.EquipmentController.Equip(invItem.Item);
            }
        }
    }
}