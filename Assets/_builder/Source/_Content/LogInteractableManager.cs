using nobodyworks.builder.carrying;
using nobodyworks.builder.interaction;
using nobodyworks.builder.utilities;
using UnityEngine;

namespace nobodyworks.builder
{
    public class LogInteractableManager : InteractableManager, ICarryable
    {
        [SerializeField]
        private GameObject _modelGameObject;
        
        [SerializeField]
        private Offset _offset;
        
        [SerializeField]
        private Rigidbody _rigidbody;
        
        public GameObject GameObject => gameObject;
        public GameObject ModelGameObject => _modelGameObject;
        public Offset Offset => _offset;
        
        public void CarryStarted()
        {
            _rigidbody.isKinematic = true;
        }

        public void CarryEnded()
        {
            _rigidbody.isKinematic = false;
        }
    }
}