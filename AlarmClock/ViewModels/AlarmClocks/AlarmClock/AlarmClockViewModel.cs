﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AlarmClock.Properties;
using AlarmClock.Tools;

namespace AlarmClock.ViewModels.AlarmClocks.AlarmClock
{
    class AlarmClockViewModel : INotifyPropertyChanged
    {
        #region Fields
        private Models.AlarmClock _currentAlarmClock;
        private string _selectedHour;
        private string _selectedMinute;
        private bool _isAlarming;
        private List<string> _hours = new List<string>();
        private List<string> _minutes = new List<string>();
        #region Commands
        private ICommand _saveNewTime;
        private ICommand _testAlarm;
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
                    _currentAlarmClock.NextTriggerDate = new DateTime(currentNextYear, currentNextMonth, currentNextDay, updatedNextHour, updatedNexMinute, 0);
                    OnPropertyChanged(nameof(_currentAlarmClock));
                    OnAlarmClockTimeUpdated(_currentAlarmClock);
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
                    Visibility = "Visible";
                    _currentAlarmClock.LastTriggerDate = DateTime.Now;
                    OnPropertyChanged(nameof(_currentAlarmClock));
                    OnAlarmClockTimeUpdated(_currentAlarmClock);
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

        /*public string IsAlarming
        {
            get { return _visibility; }
            private set { _visibility = value; }
        }*/

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
        #endregion

        #region Constructor
        public AlarmClockViewModel(Models.AlarmClock alarmClock)
        {
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
            Visibility = "Hidden";
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
        internal event AlarmClockTimeUpdatedHandler AlarmClockTimeUpdated;
        internal delegate void AlarmClockTimeUpdatedHandler(Models.AlarmClock alarmClock);

        internal virtual void OnAlarmClockTimeUpdated(Models.AlarmClock alarmClock)
        {
            AlarmClockTimeUpdated?.Invoke(alarmClock);
        }

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