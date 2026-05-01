using System;
using UnityEngine;

namespace nobodyworks.builder.movement
{
    [Serializable]
    public class MovementState
    {
        #region ===== Inspector =====

        [SerializeField]
        private int _stateId;
        
        [SerializeField]
        private float _speed = 0f;

        #endregion
        
        public int Id => _stateId;
        public float Speed => _speed;

        public MovementState(int stateId, float speed)
        {
            _stateId = stateId;
            _speed = speed;
        }
    }
}