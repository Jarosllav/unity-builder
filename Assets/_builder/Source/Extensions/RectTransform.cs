using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace nobodyworks.builder.extensions
{
    public static class RectTransformExtensions
    {
        public static void RefreshLayouts(this RectTransform rectTransform)
        {
            var layouts = rectTransform
                .GetComponentsInChildren<LayoutGroup>(includeInactive: true)?.Reverse();

            foreach (var layout in layouts)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout.transform as RectTransform);
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout.transform as RectTransform);
            }
        }
    }
}