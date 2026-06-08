using System;
using UnityEngine;
using nobodyworks.builder.items;

namespace nobodyworks.builder.building
{
    [Serializable]
    public class BuilderSettings
    {
        [SerializeField]
        private ItemDefinition _hammerItemDefinition;

        [SerializeField]
        private ElementDefinition _defaultElementDefinition;

        [SerializeField]
        private float _snapRadius = 1.5f;

        [SerializeField]
        private LayerMask _snapLayerMask;

        public ItemDefinition HammerItemDefinition => _hammerItemDefinition;
        public ElementDefinition DefaultElementDefinition => _defaultElementDefinition;
        public float SnapRadius => _snapRadius;
        public LayerMask SnapLayerMask => _snapLayerMask;
    }
}