using System;

namespace Tools.Serialization
{
    public class AlarmClockDTO
    {
        public string _guid;
        public DateTime? _lastTriggerDate;
        public DateTime _nextTriggerDate;
    }
}
