using System;
using UnityEngine;

namespace nobodyworks.builder.building
{
    public enum SnapType
    {
        None,
        Wall,
        Floor,
        Corner,
        Any
    }
    
    [Serializable]
    public class SnapPoint
    {
        [SerializeField]
        private SnapType _snapType;

        [SerializeField]
        private Vector3 _offset;

        [SerializeField]
        private Vector3 _normal;

        public SnapType SnapType => _snapType;
        public Vector3 Offset => _offset;
        public Vector3 Normal => _normal;
    }
}