using System;
using UnityEngine;
using TriInspector;

namespace nobodyworks.builder.clock
{
    [Serializable]
    public class TimeReference
    {
        private const int SECONDS_PER_DAY = 86400;
        private const int SECONDS_PER_HOUR = 3600;

        #region Inspector

        [SerializeField, Group("time")]
        protected TimeUnits _units = TimeUnits.WithoutDay;

        [SerializeField, Min(0)]
        [ShowIf(nameof(EnabledDayUnit)), Group("time")]
        protected int _days;

        [SerializeField, Min(0)]
        [ShowIf(nameof(EnabledHourUnit)), Group("time")]
        protected int _hours;

        [SerializeField, Min(0)]
        [ShowIf(nameof(EnabledMinuteUnit)), Group("time")]
        protected int _minutes;

        [SerializeField, Min(0)]
        [ShowIf(nameof(EnabledSecondUnit)), Group("time")]
        protected int _seconds;

        protected bool EnabledDayUnit => _units.HasFlag(TimeUnits.Day);
        protected bool EnabledHourUnit => _units.HasFlag(TimeUnits.Hour);
        protected bool EnabledMinuteUnit => _units.HasFlag(TimeUnits.Minute);
        protected bool EnabledSecondUnit => _units.HasFlag(TimeUnits.Second);

        #endregion

        public int Days => _days;
        public int Hours => _hours;
        public int Minutes => _minutes;
        public int Seconds => _seconds;

        public TimeReference()
        {
            
        }

        public TimeReference(TimeUnits units, int days, int hours, int minutes, int seconds)
        {
            _units = units;
            _days = days;
            _hours = hours;
            _minutes = minutes;
            _seconds = seconds;
        }

        public int AsSeconds()
        {
            var total = 0;
            if (EnabledDayUnit)    total += _days * SECONDS_PER_DAY;
            if (EnabledHourUnit)   total += _hours * SECONDS_PER_HOUR;
            if (EnabledMinuteUnit) total += _minutes * 60;
            if (EnabledSecondUnit) total += _seconds;
            return total;
        }

        public int AsMinutes()
        {
            return AsSeconds() / 60;
        }

        public int AsHours()
        {
            return AsSeconds() / 3600;
        }

        public void AddHours(int hours)
        {
            Accumulate(hours * SECONDS_PER_HOUR);
        }

        public void AddMinutes(int minutes)
        {
            Accumulate(minutes * 60);
        }

        private void Accumulate(int seconds)
        {
            var total = AsSeconds() + seconds;

            if (EnabledDayUnit)
            {
                _days = total / SECONDS_PER_DAY;
                total %= SECONDS_PER_DAY;
            }
            if (EnabledHourUnit)
            {
                _hours = total / SECONDS_PER_HOUR;
                total %= SECONDS_PER_HOUR;
            }
            if (EnabledMinuteUnit)
            {
                _minutes = total / 60;
                total %= 60;
            }
            if (EnabledSecondUnit)
            {
                _seconds = total;
            }
        }
    }
}