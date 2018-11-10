using System;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AlarmClock.Tools;

namespace AlarmClock.Models
{
    public class AlarmClock
    {
        #region Fields
        private Guid _guid;
        private DateTime? _lastTriggerDate;
        private DateTime _nextTriggerDate;
        private bool _isAlarming;
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
            set
            {
                _nextTriggerDate = value;
                if (_nextTriggerDate < DateTime.Now)
                    _nextTriggerDate = _nextTriggerDate.AddDays((DateTime.Now - _nextTriggerDate).TotalDays);
                if (_nextTriggerDate > DateTime.Now.AddDays(1))
                    _nextTriggerDate = _nextTriggerDate.AddDays(-1);
            }
        }

        public bool IsAlarming
        {
            get { return _isAlarming; }
            set { _isAlarming = value; }
        }
        #endregion

        #region Constructor
        public AlarmClock(DateTime? lastTriggerDate, DateTime nextTriggerDate)
        {
            _guid = Guid.NewGuid();
            _lastTriggerDate = lastTriggerDate;
            _nextTriggerDate = nextTriggerDate;
            _isAlarming = false;
        }

        public AlarmClock()
        {
            _guid = Guid.NewGuid();
        }

        public bool Equals(AlarmClock alarmClock)
        {
            return this.NextTriggerDate == alarmClock.NextTriggerDate;
        }

        public override string ToString()
        {
            return $"The alarm clock will trigger at {_nextTriggerDate}";
        }

        public void Alarm()
        {
            Task.Run(() =>
            {
                SoundPlayer player = new SoundPlayer(FileFolderHelper.AlarmSoundFilepath);
                player.PlayLooping();
                while (IsAlarming)
                {
                    Thread.Sleep(100);
                }
                player.Stop();
            });
            IsAlarming = true;
            LastTriggerDate = DateTime.Now;
            NextTriggerDate = NextTriggerDate.AddDays(1);
        }
        #endregion
    }
}
