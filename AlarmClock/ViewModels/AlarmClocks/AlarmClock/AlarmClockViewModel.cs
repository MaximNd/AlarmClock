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
using Managers;
using Tools;

[assembly: InternalsVisibleTo("Tests")]

namespace AlarmClock.ViewModels.AlarmClocks.AlarmClock
{
    class AlarmClockViewModel : INotifyPropertyChanged
    {
        #region Fields
        private AlarmClockForView _currentAlarmClock;
        private string _selectedHour;
        private string _selectedMinute;
        private List<string> _hours = new List<string>();
        private List<string> _minutes = new List<string>();
        #region Commands
        private ICommand _saveNewTime;
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
                    DateTime beforeDateTime = CurrentAlarmClock.NextTriggerDate;
                    int currentNextYear = beforeDateTime.Year;
                    int currentNextMonth = beforeDateTime.Month;
                    int currentNextDay = beforeDateTime.Day;
                    int updatedNextHour = Int32.Parse(SelectedHour);
                    int updatedNexMinute = Int32.Parse(SelectedMinute);
                    DateTime dateTime = new DateTime(currentNextYear, currentNextMonth, currentNextDay, updatedNextHour, updatedNexMinute, 0);
                    await Task.Run(() =>
                    {
                        // TODO delete this later
                        // fake DB delay
                        Thread.Sleep(500);
                    });
                    if (UpdateTime(dateTime))
                    {
                        Logger.Log($"User: {StationManager.CurrentUser} updated the alarm clock. Time that was Before: {beforeDateTime}, Time After: {dateTime}");
                    }
                    else
                    {
                        SystemSounds.Beep.Play();
                        MessageBox.Show("The alarm time must be unique.");
                    }
                    LoaderManager.Instance.HideLoader();
                }));
            }
        }

        public ICommand Snooze
        {
            get
            {
                return _snooze ?? (_snooze = new RelayCommand<object>(SnoozeExecute));
            }
        }

        #endregion

        public AlarmClockForView CurrentAlarmClock
        {
            get { return _currentAlarmClock; }
            set { _currentAlarmClock = value; }
        }

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
                if (!IsClockPresent)
                    return false;
                return CurrentAlarmClock.IsAlarming;
            }
            set
            {
                CurrentAlarmClock.IsAlarming = value;
                OnPropertyChanged();
            }
        }
        internal bool IsClockPresent
        {
            get { return CurrentAlarmClock != null; }
        }
        #endregion

        #region Constructor
        public AlarmClockViewModel(AlarmClockForView alarmClock)
        {
            if(alarmClock == null) return;
            CurrentAlarmClock = alarmClock;
            Initialize();
        }
        #endregion

        private void Initialize()
        {
            Hours = TimeGenerator.GenerateHours();
            Minutes = TimeGenerator.GenerateMinutes();
            int hour = CurrentAlarmClock.NextTriggerDate.Hour;
            int minute = CurrentAlarmClock.NextTriggerDate.Minute;
            _selectedHour = TimeGenerator.GetFormattedTime(hour);
            _selectedMinute = TimeGenerator.GetFormattedTime(minute);
            IsAlarming = CurrentAlarmClock.IsAlarming;
        }

        private bool UpdateTime(DateTime dateTime)
        {
            if (CheckUniqueness(dateTime))
            {
                CurrentAlarmClock.NextTriggerDate = dateTime;
                DBManager.SaveAlarmClock(CurrentAlarmClock.AlarmClock);
                OnPropertyChanged(nameof(CurrentAlarmClock));
                return true;
            }

            return false;
        }

        private bool CheckUniqueness(DateTime dateTime)
        {
            foreach (DBModels.AlarmClock alarmClock in StationManager.CurrentUser.AlarmClocks)
            {
                if (alarmClock.Guid == CurrentAlarmClock.AlarmClock.Guid)
                {
                    continue;
                }
                if (alarmClock.NextTriggerDate.TimeOfDay == dateTime.TimeOfDay)
                {
                    return false;
                }
            }

            return true;
        }

        private void SnoozeExecute(object obj)
        {
            IsAlarming = false;
            OnPropertyChanged(nameof(CurrentAlarmClock));
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
