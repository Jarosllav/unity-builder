using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace nobodyworks.builder.input
{
    public class PlayerInputProvider : IInputProvider
    {
        private readonly InputSystem_Actions _actionAsset = new();

        public PlayerInputProvider()
        {
            _actionAsset.Enable();
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