using System;

namespace nobodyworks.builder.clock
{
    public class ClockController
    {
        private const float MinutesInDay = 1440f;

        private readonly ClockSettings _settings;

        private float _minutes;
        private DayPhase _phase;

        public int Hour => (int)(_minutes / 60f);
        public int Minute => (int)(_minutes % 60f);
        public DayPhase Phase => _phase;

        public event Action OnTimeChanged;
        public event Action OnPhaseChanged;

        public ClockController(ClockSettings settings)
        {
            _settings = settings;
            _minutes = settings.StartTimeMinutes;
            CheckDayPhase();
        }

        public void Dispose()
        {
            OnTimeChanged = null;
            OnPhaseChanged = null;
        }

        public void Tick(float deltaTime)
        {
            var minutesPerSecond = _settings.GameMinutesPerRealMinute / 60f;
            _minutes += deltaTime * minutesPerSecond * _settings.TimeScale;

            if (_minutes >= MinutesInDay)
            {
                _minutes -= MinutesInDay;
            }

            OnTimeChanged?.Invoke();
            CheckDayPhase();
        }

        public void SetTime(TimeReference timeReference)
        {
            _minutes = timeReference.AsMinutes();
            OnTimeChanged?.Invoke();
            CheckDayPhase();
        }

        public TimeReference GetTime()
        {
            return new(TimeUnits.WithoutDay, 0, Hour, Minute, 0);
        }
        
        private void CheckDayPhase()
        {
            if (TryUpdateDayPhase())
            {
                OnPhaseChanged?.Invoke();
            }
        }

        private bool TryUpdateDayPhase()
        {
            var nextPhase = GetPhase((int)_minutes);

            if (_phase == nextPhase)
            {
                return false;
            }
            
            _phase = nextPhase;
            return true;
        }

        private DayPhase GetPhase(int minutes)
        {
            if (minutes >= _settings.NightTimeMinutes)
            {
                return DayPhase.Night;
            }

            if (minutes >= _settings.DayTimeMinutes)
            {
                return DayPhase.Day;
            }
            
            return DayPhase.Night;
        }
    }
}
