using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AlarmClock.Managers;
using AlarmClock.Models;
using AlarmClock.Properties;
using AlarmClock.Tools;

namespace AlarmClock.ViewModels.AlarmClocks.AlarmClock
{
    class AlarmClockViewModel : INotifyPropertyChanged
    {
        #region Fields
        private AlarmClockForView _currentAlarmClock;
        private string _selectedHour;
        private string _selectedMinute;
        private bool _isClockPresent;
        private List<string> _hours = new List<string>();
        private List<string> _minutes = new List<string>();
        #region Commands
        private ICommand _saveNewTime;
        private ICommand _testAlarm;
        private ICommand _snooze;
        #endregion
        #endregion

        #region Properties

        #region Commands
        public ICommand SaveNewTime
        {
            get
            {
                return _saveNewTime ?? (_saveNewTime = new RelayCommand<object>((object o) =>
                {

                    int currentNextYear = _currentAlarmClock.NextTriggerDate.Year;
                    int currentNextMonth = _currentAlarmClock.NextTriggerDate.Month;
                    int currentNextDay = _currentAlarmClock.NextTriggerDate.Day;
                    int updatedNextHour = Int32.Parse(SelectedHour);
                    int updatedNexMinute = Int32.Parse(SelectedMinute);
                    DateTime dateTime = new DateTime(currentNextYear, currentNextMonth, currentNextDay, updatedNextHour, updatedNexMinute, 0);
                    foreach(Models.AlarmClock ac in StationManager.CurrentUser.AlarmClocks)
                    {
                        if (ac.NextTriggerDate == dateTime)
                            //TODO Proper handling  
                            return;
                    }
                    _currentAlarmClock.NextTriggerDate = dateTime;
                    OnPropertyChanged(nameof(_currentAlarmClock));
                }));
            }
        }

        public ICommand TestAlarm
        {
            get
            {
                return _testAlarm ?? (_testAlarm = new RelayCommand<object>((object o) =>
                {
                    int currentNextYear = _currentAlarmClock.NextTriggerDate.Year;
                    int currentNextMonth = _currentAlarmClock.NextTriggerDate.Month;
                    int currentNextDay = _currentAlarmClock.NextTriggerDate.Day;
                    int updatedNextHour = Int32.Parse(SelectedHour);
                    int updatedNexMinute = Int32.Parse(SelectedMinute);
                    IsAlarming = true;
                    _currentAlarmClock.LastTriggerDate = DateTime.Now;
                    OnPropertyChanged(nameof(_currentAlarmClock));
                }));
            }
        }

        public ICommand Snooze
        {
            get
            {
                return _snooze ?? (_snooze = new RelayCommand<object>((object o) =>
                {
                    IsAlarming = false;
                    OnPropertyChanged(nameof(_currentAlarmClock));
                }));
            }
        }

        #endregion
        public List<string> Hours
        {
            get { return _hours; }
            private set { _hours = value; }
        }

        public List<string> Minutes
        {
            get { return _minutes; }
            private set { _minutes = value; }
        }

        public string SelectedHour
        {
            get { return _selectedHour; }
            set
            {
                _selectedHour = value;
                OnPropertyChanged();
            }
        }
        public string SelectedMinute
        {
            get { return _selectedMinute; }
            set
            {
                _selectedMinute = value;
                OnPropertyChanged();
            }
        }
        public bool IsAlarming
        {
            get {
                if (!this._isClockPresent)
                    return false;
                return _currentAlarmClock.IsAlarming;
            }
            set
            {
                _currentAlarmClock.IsAlarming = value;
                OnPropertyChanged();
            }
        }
        internal bool IsClockPresent
        {
            get { return this._isClockPresent; }
            set
            {
                this._isClockPresent = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public AlarmClockViewModel(AlarmClockForView alarmClock)
        {
            if(alarmClock == null)
            {
                IsClockPresent = false;
                return;
            }
            IsClockPresent = true;
            _currentAlarmClock = alarmClock;
            Initialize();
        }
        #endregion

        private void Initialize()
        {
            GenerateHours();
            GenerateMinutes();
            int hour = _currentAlarmClock.NextTriggerDate.Hour;
            int minute = _currentAlarmClock.NextTriggerDate.Minute;
            _selectedHour = hour < 10 ? $"0{hour}" : $"{hour}";
            _selectedMinute = minute < 10 ? $"0{minute}" : $"{minute}";
            IsAlarming = false;
        }

        private void GenerateHours()
        {
            for (int i = 0; i < 24; ++i)
            {
                string hour = i < 10 ? $"0{i}" : $"{i}";
                _hours.Add(hour);
            }
        }

        private void GenerateMinutes()
        {
            for (int i = 0; i < 60; ++i)
            {
                string minute = i < 10 ? $"0{i}" : $"{i}";
                _minutes.Add(minute);
            }
        }

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
