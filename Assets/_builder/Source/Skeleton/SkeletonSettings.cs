using System;
using UnityEngine;

namespace nobodyworks.builder.skeleton
{
    [Serializable]
    public class SkeletonSettings
    {
        [SerializeField]
        private BoneReference[] _bones;
        
        public BoneReference[] Bones => _bones;
    }
}