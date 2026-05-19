using System;
using nobodyworks.builder.extensions;
using UnityEngine;
using nobodyworks.builder.utilities;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace nobodyworks.builder.interaction
{
    public class InteractableManager : MonoBehaviour
    {
        #region Inspector
        
        [SerializeField]
        private LocalizedString _name;
        
        [SerializeField]
        private InteractionDefinition _interactionDefinition;
        
        [SerializeField]
        private Collider[] _colliders;
        
        [SerializeField]
        private Collider[] _triggers;
        
        #endregion
        
        public Condition UseCondition { get; } = new();
        public object Cookie { get; set; }
        
        public InteractionDefinition InteractionDefinition => _interactionDefinition;
        
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
            if ((_interactionDefinition.InteractionTypes & interactionType) == 0)
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

        public void ChangeColliders(bool enabled)
        {
            foreach (var collider in _colliders)
            {
                collider.enabled = enabled;
            }
        }

        public void ChangeTriggers(bool enabled)
        {
            foreach (var trigger in _triggers)
            {
                trigger.enabled = enabled;
            }
        }

        public virtual string GetName()
        {
            var name = _name.GetText();
#if DEBUG
            if (name == string.Empty)
            {
                return gameObject.name;
            }
#endif
            return name;
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