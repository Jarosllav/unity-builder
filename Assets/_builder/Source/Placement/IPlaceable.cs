using nobodyworks.builder.utilities;
using UnityEngine;

namespace nobodyworks.builder.placement
{
    public interface IPlaceable
    {
        GameObject GameObject { get; }
        GameObject ModelGameObject { get; }
        Vector3 FloorPosition { get; }
        
        bool CanPlace();
    }
}