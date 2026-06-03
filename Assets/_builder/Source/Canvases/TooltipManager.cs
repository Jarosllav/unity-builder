using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace nobodyworks.builder.interfaces
{
    public class TooltipManager : MonoBehaviour
    {
        private static TooltipManager _instance;
        public static TooltipManager Instance => _instance;

        #region Inspector
        
        [SerializeField]
        private Canvas _canvas;
        
        [SerializeField]
        private TMP_Text _label;
        
        [SerializeField]
        private Vector2 _offset;
        
        #endregion
        
        private RectTransform _rectTransform;
        
        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            
            _rectTransform = GetComponent<RectTransform>();
            
            Hide();
        }

        public void Update()
        {
            UpdatePosition();
        }

        public void Show(string text)
        {
            _label.text = text;
            UpdatePosition();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdatePosition()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Mouse.current.position.value,
                null,
                out var localPoint
            );

            _rectTransform.localPosition = localPoint + _offset;
        }
    }
}