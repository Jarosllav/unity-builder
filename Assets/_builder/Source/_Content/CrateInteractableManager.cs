using System;
using UnityEngine;
using nobodyworks.builder.carrying;
using nobodyworks.builder.interaction;
using nobodyworks.builder.inventories;
using nobodyworks.builder.placement;
using nobodyworks.builder.utilities;

namespace nobodyworks.builder
{
    public class CrateInteractableManager : InteractableManager, ICarryable, IPlaceable
    {
        #region Inspector

        [SerializeField]
        private InventorySettings _inventorySettings;
        
        [SerializeField]
        private GameObject _modelGameObject;
        
        [SerializeField]
        private Offset _offset;
        
        [SerializeField]
        private Vector3 _floorPosition;

        #endregion
        
        private InventoryController _inventoryController;
        
        public GameObject GameObject => gameObject;
        public GameObject ModelGameObject => _modelGameObject;
        public Offset Offset => _offset;
        public Vector3 FloorPosition => _floorPosition;
        public InventoryController InventoryController => _inventoryController;
        
        public void Awake()
        {
            _inventoryController = new(_inventorySettings);
        }

        public void CarryStarted()
        {
            ChangeColliders(false);
        }

        public void CarryEnded()
        {
            ChangeColliders(true);
        }

        public bool CanPlace()
        {
            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position + _floorPosition, 0.1f);
        }
    }
}