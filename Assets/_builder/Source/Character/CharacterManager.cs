using System;
using nobodyworks.builder.input;
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
        
        #endregion
        
        private IInputProvider _inputProvider;
        private MovementController _movementController;

        public MovementController MovementController => _movementController;
        
        public void Awake()
        {
            var movementDriver = new CharacterControllerDriver(_characterController);
            
            _movementController = new(movementDriver, transform, _eyesTransform, _eyesBoneTransform, 
                _feetBoneTransform, _groundMask, _groundDistanceCheck, _movementStates);
        }

        public void Start()
        {
            
        }

        private void OnDestroy()
        {
            _movementController.Dispose();
        }

        public void Update()
        {
            if (_inputProvider == null)
            {
                return;
            }
            
            var deltaTime = Time.deltaTime;
            
            _movementController.Tick(deltaTime);
            _movementController.Move(_inputProvider.GetMove());
            _movementController.RotateDelta(_inputProvider.GetLook());
        }

        public void SetInputProvider(IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
        }
    }
}