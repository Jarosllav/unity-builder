using System;
using System.Collections.Generic;
using nobodyworks.builder.skeleton;
using UnityEngine;
using nobodyworks.builder.utilities;

namespace nobodyworks.builder.interaction
{
    public class InteractionController : IDisposable
    {
        private readonly InteractionSettings _settings;
        private readonly Dictionary<Type, Action<InteractableManager, InteractionType>> _registeredActions = new(12);
        private readonly Dictionary<Type, Func<InteractableManager, bool>> _registeredConditions = new(12);

        private Transform _eyesTransform;
        
        private InteractableManager _currentInteractableEnteredManager;
        private Collider _currentInteractableEnteredCollider;

        private InteractableManager _currentInteractableSelectedManager;
        private Collider _currentInteractableSelectedCollider;

        private InteractableManager _usingInteractableManager;
        
        public Condition<InteractableManager> UseCondition { get; } = new();

        public event Action<InteractionController, InteractableManager> OnEntered;
        public event Action<InteractionController, InteractableManager> OnExited;
        public event Action<InteractionController, InteractableManager> OnSelected;
        public event Action<InteractionController, InteractableManager> OnDeselected;
        public event Action<InteractionController, InteractableManager, InteractionType> OnUsed;

        #region Initialization

        public InteractionController(InteractionSettings settings)
        {
            _settings = settings;
            _eyesTransform = _settings.EyesTransform;
        }

        public void Dispose()
        {
            _registeredActions.Clear();
            _registeredConditions.Clear();
            
            UseCondition.Dispose();

            OnEntered = null;
            OnExited = null;
            OnSelected = null;
            OnDeselected = null;
            OnUsed = null;
        }

        #endregion

        public void Tick(float deltaTime)
        {
            if (_eyesTransform == null)
            {
                return;
            }
            
            Debug.DrawRay(_eyesTransform.position, _eyesTransform.forward * _settings.CheckDistance, Color.red);
            
            if (Physics.SphereCast(_eyesTransform.position, 0.1f, _eyesTransform.forward, 
                    out var raycastHit, _settings.CheckDistance, _settings.InteractionMask))
            {
                SelectTrigger(raycastHit.collider);
            }
            else
            {
                DeselectTrigger();
            }
        }

        public bool Use(InteractionType interactionType)
        {
            var interactableManager = GetCurrentInteractableManager();

            if (interactableManager == null 
                || !CheckUseConditions(interactableManager, interactionType))
            {
                return false;
            }
            
            interactableManager.Use(interactionType);
            OnUsed?.Invoke(this, interactableManager, interactionType);

            var concreteType = interactableManager.GetType();
            foreach (var (registeredType, handler) in _registeredActions)
            {
                if (registeredType.IsAssignableFrom(concreteType))
                {
                    handler.Invoke(interactableManager, interactionType);
                }
            }

            return true;
        }

        public void Register<T>(Action<T, InteractionType> handler)
            where T : class
        {
            _registeredActions[typeof(T)] = (interactable, interactionType) =>
            {
                handler((T)(object)interactable, interactionType);
            };
        }

        public void Unregister<T>()
            where T : class
        {
            _registeredActions.Remove(typeof(T));
        }

        public void RegisterCondition<T>(Func<T, bool> condition)
            where T : class
        {
            _registeredConditions[typeof(T)] = interactable => condition((T)(object)interactable);
        }

        public InteractableManager GetCurrentInteractableManager()
        {
            if ( _currentInteractableSelectedManager != null)
            {
                return _currentInteractableSelectedManager;
            }

            return _currentInteractableEnteredManager;
        }
        
        private bool CheckUseConditions(InteractableManager interactableManager, InteractionType interactionType)
        {
            if (!interactableManager.CheckUsage(interactionType))
            {
                return false;
            }

            if (!UseCondition.AllTrue(interactableManager))
            {
                return false;
            }

            var concreteType = interactableManager.GetType();
            foreach (var (registeredType, condition) in _registeredConditions)
            {
                if (registeredType.IsAssignableFrom(concreteType))
                {
                    if (!condition.Invoke(interactableManager))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        #region Callbacks

        public void EnterTrigger(Collider other)
        {
            if (!CheckLayer(other.gameObject.layer))
            {
                return;
            }

            var interactableManager = other.gameObject.GetComponentInParent<InteractableManager>();

            if (interactableManager == null)
            {
                return;
            }

            _currentInteractableEnteredManager = interactableManager;
            _currentInteractableEnteredCollider = other;

            OnEntered?.Invoke(this, _currentInteractableEnteredManager);
            _currentInteractableEnteredManager.Enter();
        }

        public void ExitTrigger(Collider other)
        {
            if (!CheckLayer(other.gameObject.layer))
            {
                return;
            }

            if (_currentInteractableEnteredCollider != other)
            {
                return;
            }

            OnExited?.Invoke(this, _currentInteractableEnteredManager);
            _currentInteractableEnteredManager.Exit();

            _currentInteractableEnteredManager = null;
            _currentInteractableEnteredCollider = null;
        }

        private void SelectTrigger(Collider other)
        {
            if (_currentInteractableSelectedCollider == other)
            {
                return;
            }

            if (_currentInteractableSelectedCollider != null)
            {
                DeselectTrigger();
            }

            var interactableManager = other.gameObject.GetComponentInParent<InteractableManager>();

            if (interactableManager == null)
            {
                return;
            }

            _currentInteractableSelectedManager = interactableManager;
            _currentInteractableSelectedCollider = other;

            OnSelected?.Invoke(this, _currentInteractableSelectedManager);
            _currentInteractableSelectedManager.Select();
        }

        private void DeselectTrigger(Collider other)
        {
            if (ReferenceEquals(_currentInteractableSelectedCollider, null)                                                        
                && ReferenceEquals(_currentInteractableSelectedManager, null)) 
            {
                return;
            }

            if (_currentInteractableSelectedCollider != other)
            {
                return;
            }

            OnDeselected?.Invoke(this, _currentInteractableSelectedManager);
            _currentInteractableSelectedManager?.Deselect();

            _currentInteractableSelectedManager = null;
            _currentInteractableSelectedCollider = null;
        }

        private void DeselectTrigger()
        {
            DeselectTrigger(_currentInteractableSelectedCollider);
        }

        #endregion
        
        private bool CheckLayer(int gameObjectLayer)
        {
            return (_settings.InteractionMask & (1 << gameObjectLayer)) != 0;
        }
    }
}
