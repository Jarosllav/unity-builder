using System;
using UnityEngine;
using nobodyworks.builder.utilities;
using UnityEngine.Serialization;

namespace nobodyworks.builder.interaction
{
    public class InteractableManager : MonoBehaviour
    {
        #region Inspector
        
        [SerializeField]
        private InteractionType _interactionTypes = InteractionType.None;
        
        #endregion
        
        public Condition UseCondition { get; } = new();
        public object Cookie { get; set; }

        public event Action<InteractableManager> OnEntered;
        public event Action<InteractableManager> OnExited;
        public event Action<InteractableManager> OnSelected;
        public event Action<InteractableManager> OnDeselected;
        public event Action<InteractableManager, InteractionType> OnUsed;

        #region Initialization

        public virtual void OnDestroy()
        {
            OnEntered = null;
            OnExited = null;
            OnSelected = null;
            OnDeselected = null;
            OnUsed = null;
        }

        #endregion
        
        public bool CheckUsage(InteractionType interactionType)
        {
            if ((_interactionTypes & interactionType) == 0)
            {
                return false;
            }
            
            return UseCondition.AllTrue();
        }

        public void Use(InteractionType interactionType)
        {
            OnUsed?.Invoke(this, interactionType);
            OnUse(interactionType);
        }

        public void Enter()
        {
            OnEntered?.Invoke(this);
            OnEnter();
        }

        public void Exit()
        {
            OnExited?.Invoke(this);
            OnExit();
        }

        public void Select()
        {
            OnSelected?.Invoke(this);
            OnSelect();
        }

        public void Deselect()
        {
            OnDeselected?.Invoke(this);
            OnDeselect();
        }

        #region Callbacks

        protected virtual void OnEnter() { }

        protected virtual void OnExit() { }
        
        protected virtual void OnSelect() { }

        protected virtual void OnDeselect() { }
        
        protected virtual void OnUse(InteractionType interactionType) { }

        #endregion
    }
}