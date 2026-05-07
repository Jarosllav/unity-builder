using System;
using nobodyworks.builder.carrying;
using nobodyworks.builder.interaction;
using nobodyworks.builder.inventories;
using nobodyworks.builder.items;
using nobodyworks.builder.placement;
using nobodyworks.builder.utilities;
using UnityEngine;

namespace nobodyworks.builder
{
    public class CrateInteractableManager : InteractableManager, ICarryable, IPlaceable
    {
        #region Inspector

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
            _inventoryController = new();
            _inventoryController.Add(new(Databases.Items.GetDefinition("hammer")));
        }

        public void CarryStarted()
        {
            
        }

        public void CarryEnded()
        {
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position + _floorPosition, 0.1f);
        }
    }
}