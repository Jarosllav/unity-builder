using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace nobodyworks.builder.interfaces
{
    public class DraggableManager : MonoBehaviour
    {
        private static DraggableManager _instance;
        public static DraggableManager Instance => _instance;

        #region Inspector

        [SerializeField]
        private Canvas _canvas;

        #endregion
        
        private bool _isDragging = false;
        
        private GameObject _ghost;
        private RectTransform _ghostRectTransform;
        private object _cookie;
        
        public object Cookie => _cookie;
        public bool IsDragging => _isDragging;

        public event Action<object> OnDropped;

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public void Update()
        {
            if (!_isDragging)
            {
                return;
            }

            UpdatePosition();

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                Hide();
            }
        }

        public void Show(GameObject draggable, object cookie = null)
        {
            var size = draggable.GetComponent<RectTransform>().rect.size;
            
            _cookie = cookie;
            _ghost = GameObject.Instantiate(draggable, _canvas.transform);
            _ghostRectTransform = _ghost.GetComponent<RectTransform>();
            
            _ghostRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _ghostRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _ghostRectTransform.pivot = new Vector2(0.5f, 0.5f);
            _ghostRectTransform.sizeDelta = size;
            
            UpdatePosition();
            
            _isDragging = true;
        }

        public void Hide()
        {
            GameObject.Destroy(_ghost);
            OnDropped?.Invoke(_cookie);
            _cookie = null;
            _isDragging = false;
        }

        private void UpdatePosition()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Mouse.current.position.ReadValue(),
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out Vector2 localPoint
            );

            _ghostRectTransform.localPosition = localPoint;
        }
    }
}