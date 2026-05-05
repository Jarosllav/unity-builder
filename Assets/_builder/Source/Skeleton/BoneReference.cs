using System;
using UnityEngine;

namespace nobodyworks.builder.skeleton
{
    [Serializable]
    public class BoneReference
    {
        [SerializeField]
        private BoneDefinition _definition;
            
        [SerializeField]
        private Transform _transform;

        public BoneDefinition Definition => _definition;
        public Transform Transform => _transform;
    }
}