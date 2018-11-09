using System;
using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;
using AlarmClock.Properties;

namespace AlarmClock.Models
{
    public class AlarmClockForView : INotifyPropertyChanged
    {
        #region Fields
        private readonly AlarmClock _alarmClock;
        #endregion


        #region Constructor
        public AlarmClockForView(DateTime? lastTriggerDate, DateTime nextTriggerDate)
        {
            _alarmClock = new AlarmClock(lastTriggerDate, nextTriggerDate);
        }

        public AlarmClockForView(AlarmClock alarmClock)
        {
            _alarmClock = alarmClock;
        }
        #endregion

        #region Properties
        public AlarmClock AlarmClock
        {
            get { return _alarmClock; }
        }

        public Guid Guid
        {
            get { return _alarmClock.Guid; }
        }

        public DateTime? LastTriggerDate
        {
            get { return _alarmClock.LastTriggerDate; }
            set
            {
                _alarmClock.LastTriggerDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime NextTriggerDate
        {
            get { return _alarmClock.NextTriggerDate; }
            set
            {
                _alarmClock.NextTriggerDate = value;
                OnPropertyChanged();
            }
        }

        public bool IsAlarming
        {
            get { return _alarmClock.IsAlarming; }
            set
            {
                _alarmClock.IsAlarming = value;
                OnPropertyChanged();
            }
        }
        #endregion


        public void Alarm()
        {
            SystemSounds.Beep.Play();
            IsAlarming = true;
            LastTriggerDate = DateTime.Now;
            NextTriggerDate = NextTriggerDate.AddDays(1);
        }

        
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
