using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmClock.Models
{
    public class AlarmClock
    {
        #region Fields
        private Guid _guid;
        private DateTime? _lastTriggerDate;
        private DateTime _nextTriggerDate;
        #endregion

        #region Fields
        public Guid Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }

        public DateTime? LastTriggerDate
        {
            get { return _lastTriggerDate; }
            set { _lastTriggerDate = value; }
        }

        public DateTime NextTriggerDate
        {
            get { return _nextTriggerDate; }
            set { _nextTriggerDate = value; }
        }
        #endregion

        #region Constructor
        public AlarmClock(DateTime? lastTriggerDate, DateTime nextTriggerDate)
        {
            _guid = Guid.NewGuid();
            _lastTriggerDate = lastTriggerDate;
            _nextTriggerDate = nextTriggerDate;
        }

        public AlarmClock()
        {
        }

        public override string ToString()
        {
            return $"The alarm clock will trigger at {_nextTriggerDate}";
        }
        #endregion
    }
}
