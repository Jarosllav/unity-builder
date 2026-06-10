using System;

namespace nobodyworks.builder.interaction
{
    [Flags]
    public enum InteractionType
    {
        None = 0,
        Primary = 1 << 0,
        Secondary = 1 << 1,
        PrimaryAction = 1 << 2,
        SecondaryAction = 1 << 3,
        Force = 1 << 2
    }
}