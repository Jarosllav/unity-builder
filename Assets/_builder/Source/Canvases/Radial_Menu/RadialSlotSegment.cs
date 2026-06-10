using UnityEngine;
using UnityEngine.UI;

namespace nobodyworks.builder.interfaces
{
    public class RadialSlotSegment : Graphic
    {
        #region Inspector

        [SerializeField]
        private float _innerRadius = 60f;

        [SerializeField]
        private float _outerRadius = 160f;

        [SerializeField]
        private float _startAngle = 0f;

        [SerializeField]
        private float _endAngle = 90f;

        [SerializeField]
        private float _gapDeg = 3f;

        [SerializeField]
        private int _tessellation = 16;

        #endregion
        
        public float OuterRadius => _outerRadius;

        public void SetGap(float gap)
        {
            _gapDeg = gap;
            SetVerticesDirty();
        }

        public void SetAngles(float startAngle, float endAngle)
        {
            _startAngle = startAngle;
            _endAngle = endAngle;
            SetVerticesDirty();
        }

        public void SetOuterRadius(float outerRadius)
        {
            _outerRadius = outerRadius;
            SetVerticesDirty();
        }
        
        public override bool Raycast(Vector2 screenPoint, Camera eventCamera)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, screenPoint, eventCamera, out var local))
            {
                return false;
            }

            var angle = NormalizeAngle(Mathf.Atan2(local.y, local.x) * Mathf.Rad2Deg);
            var start = NormalizeAngle(_startAngle + _gapDeg);
            var end = NormalizeAngle(_endAngle - _gapDeg);

            if (start <= end)
            {
                return angle >= start && angle <= end;
            }

            return angle >= start || angle <= end;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            var startRad = (_startAngle + _gapDeg) * Mathf.Deg2Rad;
            var endRad = (_endAngle - _gapDeg) * Mathf.Deg2Rad;

            for (var i = 0; i <= _tessellation; i++)
            {
                var t = (float)i / _tessellation;
                var angle = Mathf.Lerp(startRad, endRad, t);

                AddPoint(vh, AngleToPoint(angle, _innerRadius));
                AddPoint(vh, AngleToPoint(angle, _outerRadius));
            }

            for (var i = 0; i < _tessellation; i++)
            {
                var b = i * 2;
                vh.AddTriangle(b, b + 1, b + 3);
                vh.AddTriangle(b, b + 3, b + 2);
            }
        }
        
        private static float NormalizeAngle(float deg)
        {
            deg %= 360f;
            if (deg < 0f)
            {
                deg += 360f;
            }

            return deg;
        }

        private void AddPoint(VertexHelper vh, Vector2 pos)
        {
            var vert = UIVertex.simpleVert;
            vert.position = new Vector3(pos.x, pos.y, 0f);
            vert.color = color;
            vh.AddVert(vert);
        }

        private static Vector2 AngleToPoint(float rad, float radius)
        {
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
        }
    }
}
