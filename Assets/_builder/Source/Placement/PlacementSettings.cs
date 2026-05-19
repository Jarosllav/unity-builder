using System;
using UnityEngine;

namespace nobodyworks.builder.placement
{
    [Serializable]
    public class PlacementSettings
    {
        [SerializeField]
        private LayerMask _placementLayerMask;
        
        [SerializeField]
        private float _maxDistance = 5f;
        
        [SerializeField]
        private float _rotateSpeed = 3f;

        [SerializeField]
        private Material _successMaterial;

        [SerializeField]
        private Material _failureMaterial;

        public LayerMask PlacementLayerMask => _placementLayerMask;
        public float MaxDistance => _maxDistance;
        public float RotateSpeed => _rotateSpeed;
        public Material SuccessMaterial => _successMaterial;
        public Material FailureMaterial => _failureMaterial;
    }
}