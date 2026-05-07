using System;
using nobodyworks.builder.carrying;
using nobodyworks.builder.interaction;
using nobodyworks.builder.utilities;
using UnityEngine;

namespace nobodyworks.builder
{
    public class CrateInteractableManager : InteractableManager, ICarryable
    {
        [SerializeField]
        private GameObject _modelGameObject;
        
        [SerializeField]
        private Offset _offset;
        
        public GameObject GameObject => gameObject;
        public GameObject ModelGameObject => _modelGameObject;
        public Offset Offset => _offset;
        
        public void CarryStarted()
        {
            
        }

        public void CarryEnded()
        {
            
        }

    }
}