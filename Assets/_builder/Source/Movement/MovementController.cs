using System;
using System.Collections.Generic;
using nobodyworks.builder.skeleton;
using nobodyworks.builder.utilities;
using UnityEngine;

namespace nobodyworks.builder.movement
{
    public sealed class MovementController
    {
        private readonly MovementSettings _settings;
        private readonly Transform _transform;
        private readonly Transform _eyesTransform;
        private readonly Transform _eyesBoneTransform;
        private readonly Transform _feetBoneTransform;
        private readonly Dictionary<int, MovementState> _states;

        private MovementDriver _driver;
        private MovementState _currentState;
        private Vector3 _inverseMotion;
        private Vector3 _velocity;
        private MovementConstraint _constraints = MovementConstraint.None;
        private float _xRotation = 0f;
        private bool _isGrounded = true;

        public MovementConstraint Constraints
        {
            get
            {
                return _constraints;
            }
            set
            {
                if (!ConstraintsCondition.AllTrue())
                {
                    return;
                }
                
                _constraints = value;
            }
        }
 
        public bool IsGrounded => _isGrounded;
        public Vector3 Motion => _inverseMotion;
        public Vector3 WorldMotion => _eyesTransform.TransformDirection(_inverseMotion);
        public MovementState CurrentState => _currentState;
        public MovementDriver Driver => _driver;
        public float EyesRotation => _xRotation;
        public Vector3 EyesDirection => _eyesTransform.forward;
        public Vector3 EyesPosition => _eyesTransform.position;

        public readonly Condition ConstraintsCondition = new();
        
        public event Action OnJumped;
        public event Action OnStateChanged;

        #region Initialization

        public MovementController(MovementSettings settings, SkeletonController skeletonController)
        {
            _settings = settings;
            _transform = _settings.Transform;
            _eyesTransform = _settings.EyesTransform;
            _eyesBoneTransform = skeletonController.GetBone(_settings.EyesBoneDefinition).Transform;
            _feetBoneTransform = skeletonController.GetBone(_settings.FeetBoneDefinition).Transform;
            
            _states = new(_settings.States.Length);

            foreach (var state in _settings.States)
            {
                _states[state.Id] = state;
            }
            
            SetState(0);
        }

        public void Dispose()
        {
            OnJumped = null;
            OnStateChanged = null;
        }

        #endregion

        public void Tick(float deltaTime)
        {
            if (!Constraints.HasFlag(MovementConstraint.Gravity))
            {
                _isGrounded = Physics.CheckSphere(_feetBoneTransform.position, _settings.GroundDistanceCheck, 
                    _settings.GroundMask);

                if (_isGrounded && _velocity.y < 0)
                {
                    _velocity.y = 0;
                }

                _velocity.y += Physics.gravity.y * deltaTime;

                _driver.ApplyMotion(_velocity * deltaTime);
            }
        }

        public void SetState(int movementStateId)
        {
            if (!_states.ContainsKey(movementStateId))
            {
                return;
            }

            if (_currentState != null && _currentState.Id == movementStateId)
            {
                return;
            }

            _currentState = _states[movementStateId];
            
            OnStateChanged?.Invoke();
        }

        public void SetDriver(MovementDriver driver)
        {
            _driver = driver;
        }
        
        public void Teleport(Vector3 position)
        {
            _driver.DisableMotion();
            
            _velocity.y = 0f;
            _transform.position = position;

            _driver.EnableMotion();
        }

        public void Jump()
        {
            if (Constraints.HasFlag(MovementConstraint.Motion) || !_isGrounded)
            {
                return;
            }

            _velocity.y = Mathf.Sqrt(-1f * _settings.JumpForce * Physics.gravity.y);
            OnJumped?.Invoke();
        }

        public void Move(Vector2 move)
        {
            var moveDirection = (move.x * _transform.right + move.y * _transform.forward).normalized;
            Move(moveDirection);
        }

        public void Move(Vector3 worldDirection)
        {
            if (Constraints.HasFlag(MovementConstraint.Motion))
            {
                worldDirection = Vector3.zero;
            }

            var motion = _currentState.Speed * Time.deltaTime * worldDirection;

            // TODO(PO): Disable movement while airborne?
            
            if (!Constraints.HasFlag(MovementConstraint.Motion))
            {
                ApplyMotion(motion);
            }

            _inverseMotion = _transform.InverseTransformDirection(worldDirection);
        }

        public void RotateDelta(Vector2 rotation)
        {
            if (!Constraints.HasFlag(MovementConstraint.Rotate))
            {
                float mouseY = rotation.y * _settings.RotateSpeed * Time.deltaTime;
                _xRotation -= mouseY;
            }

            _xRotation = Mathf.Clamp(_xRotation, -60f, 60f);
            
            RotateEyes(_xRotation);
            
            if (!Constraints.HasFlag(MovementConstraint.Rotate))
            {
                float mouseX = rotation.x * _settings.RotateSpeed * Time.deltaTime;
                _transform.Rotate(Vector3.up * mouseX);
            }
        }

        public void Rotate(Vector2 screenPosition, bool immediately = false)
        {
            var yDelta = Camera.main.transform.position.y - _transform.position.y;
            var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, yDelta));

            Rotate(worldPosition, immediately);
        }

        public void Rotate(Vector3 worldPosition, bool immediately = false)
        {
            var worldDirection = worldPosition - _transform.position;
            worldDirection.y = 0;

            RotateDirection(worldDirection, immediately);
        }

        public void RotateDirection(Vector3 worldDirection, bool immediately = false)
        {
            if (Constraints.HasFlag(MovementConstraint.Rotate))
            {
                return;
            }

            var targetRotation = Quaternion.LookRotation(worldDirection);

            if (immediately)
            {
                _transform.rotation = targetRotation;
            }
            else
            {
                _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, _settings.RotateSpeed * Time.deltaTime);
            }
        }

        public void RotateEyes(float rotation)
        {
            if (_eyesTransform == null)
            {
                return;
            }
            
            _eyesTransform.position = Vector3.Slerp(_eyesTransform.position, _eyesBoneTransform.position, Time.deltaTime * 7.5f);
            _eyesTransform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
        }
        
        private void ApplyMotion(Vector3 motion)
        {
            _driver.ApplyMotion(motion);
        }
    }
}