using System;
using nobodyworks.builder.skeleton;
using UnityEngine;

namespace nobodyworks.builder.interaction
{
    [Serializable]
    public class InteractionSettings
    {
        [SerializeField]
        private LayerMask _interactionMask;
        
        [SerializeField]
        private Transform _eyesTransform;
        
        [SerializeField]
        private float _checkDistance = 3f;
        
        public LayerMask InteractionMask => _interactionMask;
        public Transform EyesTransform => _eyesTransform;
        public float CheckDistance => _checkDistance;
    }
}