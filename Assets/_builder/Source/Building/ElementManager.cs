using System;
using UnityEngine;
using nobodyworks.builder.placement;

namespace nobodyworks.builder.building
{
    public class ElementManager : MonoBehaviour, IPlaceable
    {
        #region Inspector

        [SerializeField]
        private ElementDefinition _definition;
        
        [SerializeField]
        private GameObject _modelGameObject;
        
        [SerializeField]
        private Vector3 _floorPosition;
        
        [SerializeField]
        private SnapPoint[] _snapPoints;

        #endregion

        public GameObject GameObject => gameObject;
        public GameObject ModelGameObject => _modelGameObject;
        public Vector3 FloorPosition => _floorPosition;
        public SnapPoint[] SnapPoints => _snapPoints;

        public bool CanPlace()
        {
            return true;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            for (int i = 0; i < _snapPoints.Length; i++)
            {
                var snapPoint = _snapPoints[i];
                var worldPos = transform.TransformPoint(snapPoint.Offset);
                var worldNormal = transform.TransformDirection(snapPoint.Normal);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(worldPos, 0.08f);
                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(worldPos, worldNormal * 0.4f);
            }
        }

#endif
    }
}