using System;
using nobodyworks.builder.character;
using nobodyworks.builder.extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace nobodyworks.builder.interfaces
{
    public abstract class InterfaceManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        protected bool _showCursor = true;

        [SerializeField]
        protected bool _closeable = true;

        #endregion

        protected CanvasManager CanvasManager;
        protected GameManager GameManager;
        protected RectTransform RectTransform;

        private bool _isOpened = false;
        private bool _waitForFrame = false;

        protected CharacterManager CharacterManager => GameManager.PlayerCharacterManager;
        protected TooltipManager TooltipManager => CanvasManager.TooltipManager;
        protected DraggableManager DraggableManager => CanvasManager.DraggableManager;
        
        public bool IsOpened => _isOpened;
        public bool ShowCursor => _showCursor;
        public bool IsCloseable => _closeable;

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

        public void Initialize(CanvasManager canvasManager, GameManager gameManager)
        {
            CanvasManager = canvasManager;
            GameManager = gameManager;
            
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
        protected virtual void OnCharacterChanged(CharacterManager characterManager) { }
    }
}