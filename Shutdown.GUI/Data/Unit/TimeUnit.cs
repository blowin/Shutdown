using System;

namespace Shutdown.GUI.Data.Unit
{
    public sealed class TimeUnit : PriorityUnitBase<TimeUnit>
    {
        public static readonly TimeUnit Day = new TimeUnit(4, "Дней", TimeSpan.FromDays);
        public static readonly TimeUnit Hour = new TimeUnit(3, "Часов", TimeSpan.FromHours);
        public static readonly TimeUnit Minutes = new TimeUnit(2, "Минут", TimeSpan.FromMinutes);
        public static readonly TimeUnit Seconds = new TimeUnit(1, "Секунд", TimeSpan.FromSeconds);

        private Func<double, TimeSpan> _asTimeSpan;

        private TimeUnit(int priority, string name, Func<double, TimeSpan> asTimeSpan)
            : base(name, priority)
        {
            _asTimeSpan = asTimeSpan;
        }

        public TimeSpan AsTimeSpan(double item) => _asTimeSpan(item);
    }
}