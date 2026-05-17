using nobodyworks.builder.extensions;
using UnityEngine;
using nobodyworks.builder.movement;
using nobodyworks.builder.utilities;

namespace nobodyworks.builder.placement
{
    public class PlacementController
    {
        private readonly PlacementSettings _settings;
        private readonly MovementController _movementController;
        
        private IPlaceable _placeable;
        private GameObject _ghostGameObject;
        private TriggerHandlers _ghostTriggerHandlers;
        private MaterialsSnapshot _materialsSnapshot;
        private bool _lastCanPlace = false;
        
        public PlacementController(PlacementSettings settings, MovementController movementController)
        {
            _settings = settings;
            _movementController = movementController;
        }
        
        public void Tick(float deltaTime)
        {
            if (_placeable == null)
            {
                return;    
            }
            
            if (Physics.Raycast(_movementController.EyesPosition, _movementController.EyesDirection, out var hit,
                    _settings.MaxDistance, _settings.PlacementLayerMask))
            {
                _ghostGameObject.transform.position = hit.point - _placeable.FloorPosition;
            }

            TryUpdateGhostMaterials();
        }

        public void Create(IPlaceable placeable)
        {
            if (_placeable != null)
            {
                return;
            }
            
            _placeable = placeable;
            _ghostGameObject = GameObject.Instantiate(_placeable.ModelGameObject);
            _ghostTriggerHandlers = _ghostGameObject.AddComponent<TriggerHandlers>();
            _lastCanPlace = true;
            
            var rigidbody = _ghostGameObject.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            
            _materialsSnapshot = new MaterialsSnapshot(_ghostGameObject);
            TryUpdateGhostMaterials(true);
        }

        public void Destroy()
        {
            _placeable.GameObject.transform.SetPositionAndRotation(
                _ghostGameObject.transform.position,
                _ghostGameObject.transform.rotation
            );
            
            GameObject.Destroy(_ghostGameObject);

            _ghostTriggerHandlers = null;
            _materialsSnapshot = null;
            _placeable = null;
        }

        public bool CanPlace()
        {
            if (_placeable == null)
            {
                return true;
            }
            
            return _placeable.CanPlace() && !_ghostTriggerHandlers.IsColliding;
        }

        public void Rotate(bool clockwise)
        {
            if (_placeable == null)
            {
                return;
            }
            
            var angles = _settings.RotateSpeed * Time.deltaTime;
            _ghostGameObject.transform.Rotate(Vector3.up, clockwise ? angles : -angles);
        }

        private void TryUpdateGhostMaterials(bool force = false)
        {
            var canPlace = CanPlace();

            if (force || _lastCanPlace != canPlace)
            {
                _lastCanPlace = canPlace;
                _materialsSnapshot.Override(canPlace ? _settings.SuccessMaterial : _settings.FailureMaterial);
            }
        }
    }
}