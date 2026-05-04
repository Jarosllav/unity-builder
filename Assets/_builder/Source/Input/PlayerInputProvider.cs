using System;
using nobodyworks.builder.character;
using nobodyworks.builder.movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace nobodyworks.builder.input
{
    public class PlayerInputProvider : MonoBehaviour, IInputProvider
    {
        private InputSystem_Actions _actionAsset;
        private CharacterManager _characterManager;
        private MovementController _movementController;
        
        public void Awake()
        {
            _actionAsset = new();
            _characterManager = GetComponent<CharacterManager>();
            _movementController = _characterManager.MovementController;
        }

        public void Start()
        {
            _actionAsset.Enable();
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
    }
}