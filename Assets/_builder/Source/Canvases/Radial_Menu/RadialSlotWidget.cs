using System;
using DG.Tweening;
using nobodyworks.builder.building;
using UnityEngine;
using UnityEngine.EventSystems;

namespace nobodyworks.builder.interfaces
{
    public class RadialSlotWidget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Inspector

        [SerializeField]
        private RadialSlotSegment _segment;

        #endregion

        private Tween _tween;
        private float _outerRadius;

        public ElementDefinition Definition { get; set; }
        
        public RadialSlotSegment Segment => _segment;
        
        public event Action<RadialSlotWidget> OnSelected;
        
        private void Awake()
        {
            _outerRadius = _segment.OuterRadius;
        }

        private void OnDisable()
        {
            _tween?.Kill();
            _segment.SetOuterRadius(_outerRadius);
        }

        private void OnDestroy()
        {
            OnSelected = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tween?.Kill();
            _tween = DOTween.To(() => _segment.OuterRadius, x => _segment.SetOuterRadius(x), _outerRadius + 40f, 0.2f);
            OnSelected?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tween?.Kill();
            _tween = DOTween.To(() => _segment.OuterRadius, x => _segment.SetOuterRadius(x), _outerRadius, 0.1f);
        }
    }
}