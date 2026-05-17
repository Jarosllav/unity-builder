using System;
using UnityEngine;

namespace nobodyworks.builder.placement
{
    public class MaterialsSnapshot
    {
        private readonly Renderer[] _renderers;
        private readonly Material[][] _originalMaterials;
        private readonly Material[][] _overrideArrays;

        public MaterialsSnapshot(GameObject gameObject)
        {
            _renderers = gameObject.GetComponentsInChildren<Renderer>();
            _originalMaterials = new Material[_renderers.Length][];
            _overrideArrays = new Material[_renderers.Length][];

            for (var i = 0; i < _renderers.Length; i++)
            {
                _originalMaterials[i] = _renderers[i].sharedMaterials;
                _overrideArrays[i] = new Material[_originalMaterials[i].Length];
            }
        }

        public void Override(Material material)
        {
            for (var i = 0; i < _renderers.Length; i++)
            {
                Array.Fill(_overrideArrays[i], material);
                _renderers[i].sharedMaterials = _overrideArrays[i];
            }
        }

        public void Restore()
        {
            for (var i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].sharedMaterials = _originalMaterials[i];
            }
        }
    }
}
