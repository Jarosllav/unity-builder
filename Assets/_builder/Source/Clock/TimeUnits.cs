using System;

namespace nobodyworks.builder.clock
{
    [Flags]
    public enum TimeUnits : byte
    {
        None = 0,
        Day = 1,
        Hour = 2,
        Minute = 4,
        Second = 8,

        All = Day | Hour | Minute | Second,
        WithoutDay = Hour | Minute | Second,
    };
}