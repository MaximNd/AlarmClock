using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmClock.Tools.Serialization
{
    public class AlarmClockDTO
    {
        public DateTime? _lastTriggerDate;
        public DateTime _nextTriggerDate;
    }
}
