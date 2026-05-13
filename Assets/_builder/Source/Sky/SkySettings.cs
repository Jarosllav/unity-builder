using System;
using UnityEngine;

namespace nobodyworks.builder.sky
{
    [Serializable]
    public class SkySettings
    {
        [SerializeField]
        private Transform _skyTransform;

        [SerializeField]
        private Material _grassMaterial;

        [SerializeField]
        private SkyScenerySettings _dayScenerySettings;
        
        [SerializeField]
        private SkyScenerySettings _nightScenerySettings;

        public Transform SkyTransform => _skyTransform;
        public Material GrassMaterial => _grassMaterial;
        public SkyScenerySettings DayScenerySettings => _dayScenerySettings;
        public SkyScenerySettings NightScenerySettings => _nightScenerySettings;
    }
}
