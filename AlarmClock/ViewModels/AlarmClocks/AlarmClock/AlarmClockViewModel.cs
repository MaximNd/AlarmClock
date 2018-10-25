using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AlarmClock.Properties;

namespace AlarmClock.ViewModels.AlarmClocks.AlarmClock
{
    class AlarmClockViewModel
    {
        #region Fields
        private readonly Models.AlarmClock _currentAlarmClock;
        #endregion

        #region Properties

        public DateTime LastTriggerDate
        {
            get { return _currentAlarmClock.LastTriggerDate; }
            set
            {
                _currentAlarmClock.LastTriggerDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime NextTriggerDate
        {
            get { return _currentAlarmClock.NextTriggerDate; }
            set
            {
                _currentAlarmClock.NextTriggerDate = value;
                OnPropertyChanged();
            }
        }
        #endregion



        #region Constructor
        public AlarmClockViewModel(Models.AlarmClock alarmClock)
        {
            _currentAlarmClock = alarmClock;
        }
        #endregion
        #region EventsAndHandlers
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        internal virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion
    }
}
