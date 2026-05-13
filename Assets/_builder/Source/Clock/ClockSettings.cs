using System;
using TriInspector;
using UnityEngine;

namespace nobodyworks.builder.clock
{
    [Serializable]
    public class ClockSettings
    {
        [SerializeField] 
        private int _gameMinuteToRealMinute = 10;
        
        [SerializeField, Group("Start time")]
        private TimeReference _startTimeReference;
        
        [SerializeField, Group("Day time")]
        private TimeReference _dayTimeReference;
        
        [SerializeField, Group("Night time")]
        private TimeReference _nightTimeReference;
        
        [SerializeField]
        private float _timeScale = 1f;

        public int GameMinutesPerRealMinute => _gameMinuteToRealMinute;
        public float TimeScale => _timeScale;
        public int StartTimeMinutes => _startTimeReference.AsMinutes();
        public int DayTimeMinutes => _dayTimeReference.AsMinutes();
        public TimeReference DayTimeReference => _dayTimeReference;
        public int NightTimeMinutes => _nightTimeReference.AsMinutes();
        public TimeReference NightTimeReference => _nightTimeReference;
    }
}
