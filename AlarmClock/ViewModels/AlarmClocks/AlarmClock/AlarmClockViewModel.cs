using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
                return _saveNewTime ?? (_saveNewTime = new RelayCommand<object>(async (object o) =>
                {
                    LoaderManager.Instance.ShowLoader();
                    int currentNextYear = _currentAlarmClock.NextTriggerDate.Year;
                    int currentNextMonth = _currentAlarmClock.NextTriggerDate.Month;
                    int currentNextDay = _currentAlarmClock.NextTriggerDate.Day;
                    int updatedNextHour = Int32.Parse(SelectedHour);
                    int updatedNexMinute = Int32.Parse(SelectedMinute);
                    DateTime dateTime = new DateTime(currentNextYear, currentNextMonth, currentNextDay, updatedNextHour, updatedNexMinute, 0);
                    foreach (Models.AlarmClock alarmClock in StationManager.CurrentUser.AlarmClocks)
                    {
                        if (alarmClock.Guid != _currentAlarmClock.AlarmClock.Guid && alarmClock.NextTriggerDate == dateTime)
                        {
                            SystemSounds.Beep.Play();
                            MessageBox.Show("The alarm time must be unique.");
                            return;
                        }
                    }
                    await Task.Run(() =>
                    {
                        // TODO delete this later
                        // fake DB delay
                        Thread.Sleep(500);
                    });
                    Logger.Log($"User: {StationManager.CurrentUser} updated the alarm clock. Time that was Before: {_currentAlarmClock.AlarmClock.NextTriggerDate}, Time After: {dateTime}");
                    _currentAlarmClock.NextTriggerDate = dateTime;
                    OnPropertyChanged(nameof(_currentAlarmClock));
                    LoaderManager.Instance.HideLoader();
                }));
            }
        }

        public ICommand TestAlarm
        {
            get
            {
                return _testAlarm ?? (_testAlarm = new RelayCommand<object>((object o) =>
                {
                    SystemSounds.Asterisk.Play();
                    IsAlarming = true;
                    _currentAlarmClock.NextTriggerDate = _currentAlarmClock.NextTriggerDate.AddDays(1);
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
            Hours = TimeGenerator.GenerateHours();
            Minutes = TimeGenerator.GenerateMinutes();
            int hour = _currentAlarmClock.NextTriggerDate.Hour;
            int minute = _currentAlarmClock.NextTriggerDate.Minute;
            _selectedHour = TimeGenerator.GetFormattedTime(hour);
            _selectedMinute = TimeGenerator.GetFormattedTime(minute);
            IsAlarming = _currentAlarmClock.IsAlarming;
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
