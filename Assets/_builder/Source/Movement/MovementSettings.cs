using System;
using nobodyworks.builder.skeleton;
using UnityEngine;
using UnityEngine.Serialization;

namespace nobodyworks.builder.movement
{
    [Serializable]
    public class MovementSettings
    {
        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private Transform _eyesTransform;
        
        [SerializeField]
        private BoneDefinition _eyesBoneDefinition;
        
        [SerializeField]
        private BoneDefinition _feetBoneDefinition;
        
        [SerializeField]
        private LayerMask _groundMask;
        
        [SerializeField]
        private float _groundDistanceCheck;
        
        [SerializeField]
        private float _rotateSpeed = 10f;
        
        [SerializeField]
        private float _jumpForce = 2f;
        
        [SerializeField]
        private MovementState[] _states;
        
        public Transform Transform => _transform;
        public Transform EyesTransform => _eyesTransform;
        public BoneDefinition EyesBoneDefinition => _eyesBoneDefinition;
        public BoneDefinition FeetBoneDefinition => _feetBoneDefinition;
        public LayerMask GroundMask => _groundMask;
        public float GroundDistanceCheck => _groundDistanceCheck;
        public float RotateSpeed => _rotateSpeed;
        public float JumpForce => _jumpForce;
        public MovementState[] States => _states;
    }
}