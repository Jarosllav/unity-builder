using System;
using UnityEngine;

namespace nobodyworks.builder.utilities
{
    public class TriggerHandlers : MonoBehaviour
    {
        private bool _isColliding;

        public bool IsColliding => _isColliding;
        
        public event Action<Collider> OnEntered;
        public event Action<Collider> OnExited;

        public void FixedUpdate()
        {
            _isColliding = false;
        }
    
        public void OnDestroy()
        {
            OnEntered = null;
            OnExited = null;
        }

        public void OnTriggerEnter(Collider other)
        {
            OnEntered?.Invoke(other);
        }

        public void OnTriggerStay(Collider other)
        {
            _isColliding = true;
        }

        public void OnTriggerExit(Collider other)
        {
            OnExited?.Invoke(other);
        }
    }
}