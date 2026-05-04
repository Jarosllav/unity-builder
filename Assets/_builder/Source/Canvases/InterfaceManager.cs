using System;
using nobodyworks.builder.extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace nobodyworks.builder
{
    public abstract class InterfaceManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        protected bool _showCursor = true;

        #endregion

        protected CanvasManager CanvasManager;
        protected RectTransform RectTransform;

        private bool _isOpened = false;
        private bool _waitForFrame = false;

        public bool IsOpened => _isOpened;
        public bool ShowCursor => _showCursor;

        public event Action<InterfaceManager> OnOpened;
        public event Action<InterfaceManager> OnOpening;
        public event Action<InterfaceManager> OnClosed;
        public event Action<InterfaceManager> OnClosing;

        #region Initialization

        public virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        public virtual void Start()
        {
            OnStarted();
        }

        public virtual void OnDestroy()
        {
            if (_isOpened)
            {
                Close();
            }
            
            OnDestroyed();
            
            OnOpened = null;
            OnOpening = null;
            OnClosed = null;
            OnClosing = null;
        }

        #endregion

        public void LateUpdate()
        {
            if (_waitForFrame)
            {
                RectTransform.RefreshLayouts();
                _waitForFrame = false;
            }
        }

        public void Initialize(CanvasManager canvasManager)
        {
            CanvasManager = canvasManager;

            OnInitialized();
        }

        public void Toggle()
        {
            if (_isOpened)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public void Open()
        {
            OnOpening?.Invoke(this);

            gameObject.SetActive(true);
            _isOpened = true;
            OnOpened?.Invoke(this);
            
            _waitForFrame = true;
        }

        public void Close()
        {
            OnClosing?.Invoke(this);

            gameObject.SetActive(false);
            _isOpened = false;

            OnClosed?.Invoke(this);
        }

        public void RefreshLayout()
        {
            _waitForFrame = true;
        }
        
        protected virtual void OnInitialized() { }
        protected virtual void OnStarted() { }
        protected virtual void OnDestroyed() { }
    }
}