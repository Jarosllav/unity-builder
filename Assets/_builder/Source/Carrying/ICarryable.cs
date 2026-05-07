using UnityEngine;
using nobodyworks.builder.utilities;

namespace nobodyworks.builder.carrying
{
    public interface ICarryable
    {
        GameObject GameObject { get; }
        GameObject ModelGameObject { get; }
        Offset Offset { get; }
        
        void CarryStarted();
        void CarryEnded();
    }
}