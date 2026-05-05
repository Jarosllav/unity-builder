using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace nobodyworks.builder.skeleton
{
    public class SkeletonController
    {
        private readonly SkeletonSettings _settings;
        
        public SkeletonController(SkeletonSettings settings)
        {
            _settings = settings;    
        }

        public BoneReference GetBone(int id)
        {
            foreach (var bone in _settings.Bones)
            {
                if (bone.Definition.ID == id)
                {
                    return bone;
                }
            }
            
            return null;
        }
        
        public BoneReference GetBone(string code)
        {
            foreach (var bone in _settings.Bones)
            {
                if (bone.Definition.Code == code)
                {
                    return bone;
                }
            }
            
            return null;
        }

        public BoneReference GetBone(BoneDefinition definition)
        {
            foreach (var bone in _settings.Bones)
            {
                if (bone.Definition == definition)
                {
                    return bone;
                }
            }
            
            return null;
        }
    }
}