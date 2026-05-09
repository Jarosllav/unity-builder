using nobodyworks.builder.utilities;
using UnityEngine;

namespace nobodyworks.builder.extensions
{
    public static class TransformExtensions
    {
        public static void SetOffset(this Transform transform, Offset offset)
        {
            var offsetPosition = offset.Position;
            var offsetRotation = offset.Angles;

            transform.SetPositionAndRotation(offsetPosition, Quaternion.Euler(offsetRotation));

        }

        public static void SetLocalOffset(this Transform transform, Offset offset)
        {
            var offsetPosition = offset.Position;
            var offsetRotation = offset.Angles;

            transform.SetLocalPositionAndRotation(offsetPosition, Quaternion.Euler(offsetRotation));
        }

        public static void DestroyChildren(this Transform transform)
        {
            var childCount = transform.childCount;

            for (int i = 0; i < childCount; ++i)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}