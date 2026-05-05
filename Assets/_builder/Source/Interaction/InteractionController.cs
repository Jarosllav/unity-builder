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
        private readonly Dictionary<Type, Action<InteractableManager>> _registeredActions = new(12);
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
        public event Action<InteractionController, InteractableManager> OnUsed;

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
            
            interactableManager.Use();
            OnUsed?.Invoke(this, interactableManager);
            
            if (_registeredActions.TryGetValue(interactableManager.GetType(), out var handler))
            {
                handler.Invoke(interactableManager);
            }
            
            return true;
        }

        public void Register<TInteractableManager>(Action<TInteractableManager> handler)
            where TInteractableManager : InteractableManager
        {
            _registeredActions[typeof(TInteractableManager)] = interactable =>
            {
                handler((TInteractableManager)interactable);
            };
        }

        public void Unregister<TInteractableManager>(Action<TInteractableManager> handler)
        {
            _registeredActions.Remove(typeof(TInteractableManager));
        }

        public void RegisterCondition<TInteractableManager>(Func<TInteractableManager, bool> condition)
            where TInteractableManager : InteractableManager
        {
            _registeredConditions[typeof(TInteractableManager)] = interactable => condition((TInteractableManager)interactable);
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
            
            if (_registeredConditions.TryGetValue(interactableManager.GetType(), out var condition))
            {
                if (!condition.Invoke(interactableManager))
                {
                    return false;
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
            if (_currentInteractableSelectedCollider == null && _currentInteractableSelectedManager == null)
            {
                return;
            }

            if (_currentInteractableSelectedCollider != other)
            {
                return;
            }

            OnDeselected?.Invoke(this, _currentInteractableSelectedManager);
            _currentInteractableSelectedManager.Deselect();

            _currentInteractableSelectedManager = null;
            _currentInteractableSelectedCollider = null;
        }

        private void DeselectTrigger()
        {
            DeselectTrigger(_currentInteractableSelectedCollider);
        }

        #endregion

        private InteractableManager GetCurrentInteractableManager()
        {
            if ( _currentInteractableSelectedManager != null)
            {
                return _currentInteractableSelectedManager;
            }

            return _currentInteractableEnteredManager;
        }

        private bool CheckLayer(int gameObjectLayer)
        {
            return (_settings.InteractionMask & (1 << gameObjectLayer)) != 0;
        }
    }
}
