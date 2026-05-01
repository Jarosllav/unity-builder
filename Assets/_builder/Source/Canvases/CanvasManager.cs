using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace nobodyworks.builder
{
    [DefaultExecutionOrder((int)ExecutionOrder.Late)]
    public class CanvasManager : MonoBehaviour
    {
        private static CanvasManager _instance;
        public static CanvasManager Instance => _instance;
        
        private readonly List<InterfaceManager> _interfacesManagers = new(12);
        private readonly List<InterfaceManager> _openedInterfaces = new(4);
        
        private EventSystem _eventSystem;
        private int _cursorRefCount = 0;

        #region Initialization

        public void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }

            if (_instance == null)
            {
                _instance = this;
            }
            
            _eventSystem = FindAnyObjectByType<EventSystem>();
            
            var foundInterfaces = FindObjectsByType<InterfaceManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var interfaceManager in foundInterfaces)
            {
                RegisterInterface(interfaceManager);
                
                var wasActive = interfaceManager.gameObject.activeSelf;
                
                interfaceManager.gameObject.SetActive(true);

                if (wasActive)
                {
                    interfaceManager.Open();
                }
            }
        }

        public void Start()
        {
            foreach (var interfaceManager in _interfacesManagers)
            {
                interfaceManager.Initialize(this);

                if (!interfaceManager.IsOpened)
                {
                    interfaceManager.gameObject.SetActive(false);
                }
            }
        }

        public void OnDestroy()
        {
            _openedInterfaces.Clear();
        }
        
        #endregion
        
        public TInterface GetInterface<TInterface>()
            where TInterface : InterfaceManager
        {
            foreach (var interfaceManager in _interfacesManagers)
            {
                if (interfaceManager is TInterface typedInterfaceManager)
                {
                    return typedInterfaceManager;
                }
            }

            return null;
        }
        
        public void RegisterInterface(InterfaceManager interfaceManager)
        {
            if (_interfacesManagers.Contains(interfaceManager))
            {
                return;
            }
            
            _interfacesManagers.Add(interfaceManager);
            
            interfaceManager.OnOpened += InterfaceOpenedHandler;
            interfaceManager.OnClosed += InterfaceClosedHandler;
        }

        public void UnregisterInterface(InterfaceManager interfaceManager)
        {
            if (!_interfacesManagers.Remove(interfaceManager))
            {
                return;
            }
            
            _openedInterfaces.Remove(interfaceManager);
                
            interfaceManager.OnOpened -= InterfaceOpenedHandler;
            interfaceManager.OnClosed -= InterfaceClosedHandler;
        }

        private void UpdateCursorState()
        {
            Cursor.lockState = _cursorRefCount > 0 ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        private void InterfaceOpenedHandler(InterfaceManager interfaceManager)
        {
            if (!_openedInterfaces.Contains(interfaceManager))
            {
                _openedInterfaces.Add(interfaceManager);
            }

            if (interfaceManager.ShowCursor)
            {
                _cursorRefCount++;
                UpdateCursorState();
            }
        }

        private void InterfaceClosedHandler(InterfaceManager interfaceManager)
        {
            if (!_openedInterfaces.Remove(interfaceManager))
            {
                return;
            }
            
            if (interfaceManager.ShowCursor)
            {
                _cursorRefCount--;
                UpdateCursorState();
            }
        }
    }
}