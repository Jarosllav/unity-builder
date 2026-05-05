using System;
using UnityEngine;

namespace nobodyworks.builder.skeleton
{
    [CreateAssetMenu(menuName = "Game/Definitions/Skeleton/Bone Definition")]
    public class BoneDefinition : ScriptableObject
    {
        [SerializeField]
        private int _id;
        
        [SerializeField]
        private string _code = string.Empty;
        
        public int ID => _id;
        public string Code => _code;
    }
}