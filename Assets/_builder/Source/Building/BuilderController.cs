using UnityEngine;
using nobodyworks.builder.placement;

namespace nobodyworks.builder.building
{
    public class BuilderController
    {
        private readonly BuilderSettings _settings;
        private readonly PlacementController _placementController;
        
        private ElementDefinition _selectedElement;

        private bool _isEnabled = false;
        private GameObject _elementGameObject;
        private ElementManager _elementManager;

        public bool IsEnabled => _isEnabled;
        
        #region Initialization

        public BuilderController(BuilderSettings settings, PlacementController placementController)
        {
            _settings = settings;
            _placementController = placementController;
            SetElement(_settings.DefaultElementDefinition);
        }

        #endregion

        public void Tick(float deltaTime)
        {
            if (!_isEnabled)
            {
                return;
            }

            var ghost = _placementController.GhostGameObject;

            if (ghost == null || !ghost.activeSelf)
            {
                return;
            }

            if (!TryFindSnap(ghost, out var ghostSnap, out var worldSnap, out var worldManager))
            {
                return;
            }

            var snapPos = worldManager.transform.TransformPoint(worldSnap.Offset);
            ghost.transform.position = snapPos - ghost.transform.rotation * ghostSnap.Offset;
        }

        public void TryPlace()
        {
            if (!_placementController.CanPlace())
            {
                return;
            }

            _placementController.Destroy();
            CreatePending();
        }

        public void SetEnabled(bool enabled)
        {
            if (_isEnabled == enabled)
            {
                return;
            }

            _isEnabled = enabled;

            if (_isEnabled)
            {
                CreatePending();
                return;
            }

            _placementController.Cancel();
            GameObject.Destroy(_elementGameObject);
            _elementGameObject = null;
            _elementManager = null;
        }

        public void SetElement(ElementDefinition element)
        {
            var wasEnabled = _isEnabled;
            
            if (_selectedElement != element)
            {
                SetEnabled(false);
            }
            
            _selectedElement = element;
            SetEnabled(wasEnabled);
        }

        private void CreatePending()
        {
            _elementGameObject = GameObject.Instantiate(_selectedElement.Prefab);
            _elementManager = _elementGameObject.GetComponent<ElementManager>();
            _placementController.Create(_elementManager);
        }

        private bool TryFindSnap(GameObject ghost, out SnapPoint bestGhostSnap, out SnapPoint bestWorldSnap, out ElementManager bestWorldManager)
        {
            bestGhostSnap = null;
            bestWorldSnap = null;
            bestWorldManager = null;

            var bestDist = float.MaxValue;
            var overlap = Physics.OverlapSphere(ghost.transform.position, _settings.SnapRadius, _settings.SnapLayerMask);

            foreach (var col in overlap)
            {
                var worldManager = col.GetComponentInParent<ElementManager>();
                if (worldManager == null || worldManager == _elementManager)
                {
                    continue;
                }

                foreach (var worldSnap in worldManager.SnapPoints)
                {
                    var worldSnapPos = worldManager.transform.TransformPoint(worldSnap.Offset);
                    var worldNormal = worldManager.transform.TransformDirection(worldSnap.Normal);

                    foreach (var ghostSnap in _elementManager.SnapPoints)
                    {
                        if (!AreCompatible(ghostSnap.SnapType, worldSnap.SnapType))
                        {
                            continue;
                        }

                        var ghostNormal = ghost.transform.TransformDirection(ghostSnap.Normal);
                        if (Vector3.Dot(worldNormal, ghostNormal) >= 0f)
                        {
                            continue;
                        }

                        var ghostSnapWorldPos = ghost.transform.TransformPoint(ghostSnap.Offset);
                        var dist = Vector3.Distance(ghostSnapWorldPos, worldSnapPos);
                        if (dist >= bestDist)
                        {
                            continue;
                        }

                        bestDist = dist;
                        bestGhostSnap = ghostSnap;
                        bestWorldSnap = worldSnap;
                        bestWorldManager = worldManager;
                    }
                }
            }

            return bestWorldManager != null && bestDist < _settings.SnapRadius;
        }

        private static bool AreCompatible(SnapType a, SnapType b)
        {
            if (a == SnapType.None || b == SnapType.None)
            {
                return false;
            }

            if (a == SnapType.Any || b == SnapType.Any)
            {
                return true;
            }

            return a == b;
        }
    }
}
