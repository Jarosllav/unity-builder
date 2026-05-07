using System;

namespace nobodyworks.builder.interaction
{
    [Flags]
    public enum InteractionType
    {
        None = 0,
        Primary = 1 << 0,
        Secondary = 1 << 1,
        Force = 1 << 2
    }
}