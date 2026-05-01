using System;

namespace nobodyworks.builder.movement
{
    [Flags]
    public enum MovementConstraint : byte
    {
        None = 0,
        Motion = 1,
        Rotate = 2,
        Gravity = 4,
        
        FullMotion = Motion | Rotate,
        All = Motion | Rotate | Gravity
    }
}