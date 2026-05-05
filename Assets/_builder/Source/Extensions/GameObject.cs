using UnityEngine;

namespace nobodyworks.builder.extensions
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject)
            where T : Component
        {
            var component = gameObject.GetComponent<T>();

            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        public static void ChangeChildrenLayer(this GameObject gameObject, int layer)
        {
            var children = gameObject.GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (var child in children)
            {
                child.gameObject.layer = layer;
            }
        }
    }
}
