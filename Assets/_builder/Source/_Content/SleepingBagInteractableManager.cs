using nobodyworks.builder.carrying;
using nobodyworks.builder.interaction;
using nobodyworks.builder.placement;
using nobodyworks.builder.utilities;
using UnityEngine;

namespace nobodyworks.builder
{
    public class SleepingBagInteractableManager : InteractableManager, ICarryable, IPlaceable
    {
        #region Inspector

        [SerializeField]
        private GameObject _modelGameObject;
        
        [SerializeField]
        private Offset _offset;
        
        [SerializeField]
        private Vector3 _floorPosition;

        #endregion
        
        public GameObject GameObject => gameObject;
        public GameObject ModelGameObject => _modelGameObject;
        public Offset Offset => _offset;
        public Vector3 FloorPosition => _floorPosition;
        
        public bool CanPlace()
        {
            return true;
        }

        public void CarryStarted()
        {
            
        }

        public void CarryEnded()
        {
            
        }

        protected override void OnUse(InteractionType interactionType)
        {
            if (interactionType == InteractionType.Primary)
            {
                var time = GameManager.Instance.ClockController.GetTime();
                
                time.AddHours(8);
                
                GameManager.Instance.ClockController.SetTime(time);
            }
        }
    }
}