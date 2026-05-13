using System;
using UnityEngine;

namespace nobodyworks.builder.sky
{
    [Serializable]
    public class SkyScenerySettings
    {
        [SerializeField]
        private Material _skyboxMaterial;

        [SerializeField]
        private GameObject _celestialGameObject;

        [SerializeField]
        private Light _celestialLight;
        
        [SerializeField]
        private Color _fogColor = Color.gray;

        [SerializeField, Min(0f)]
        private float _fogDensity = 0.01f;

        [SerializeField]
        private Color _ambientColor = Color.white;

        [SerializeField, Min(0f)]
        private float _ambientIntensity = 1f;

        [SerializeField, Min(0f)]
        private float _reflectionIntensity = 1f;

        [Header("Grass asset specific")]
        [SerializeField]
        private Color _grassSubsurfaceColor = Color.white;

        [SerializeField, Min(0f)]
        private float _grassDistanceFadeStart = 20f;

        [SerializeField, Min(0f)]
        private float _grassDistanceFadeEnd = 50f;

        public Material SkyboxMaterial => _skyboxMaterial;
        public GameObject CelestialGameObject => _celestialGameObject;
        public Light CelestialLight => _celestialLight;
        public Color FogColor => _fogColor;
        public float FogDensity => _fogDensity;
        public Color AmbientColor => _ambientColor;
        public float AmbientIntensity => _ambientIntensity;
        public float ReflectionIntensity => _reflectionIntensity;
        public Color GrassSubsurfaceColor => _grassSubsurfaceColor;
        public float GrassDistanceFadeStart => _grassDistanceFadeStart;
        public float GrassDistanceFadeEnd => _grassDistanceFadeEnd;
    }
}