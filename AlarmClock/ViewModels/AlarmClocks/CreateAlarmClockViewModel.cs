using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AlarmClock.Managers;
using AlarmClock.Properties;
using AlarmClock.Tools;

namespace AlarmClock.ViewModels.AlarmClocks
{
    internal class CreateAlarmClockViewModel : INotifyPropertyChanged
    {
        #region Fields

        private DateTime _newDateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
        private string _selectedHour;
        private string _selectedMinute;
        private List<string> _hours = new List<string>();
        private List<string> _minutes = new List<string>();
        #region Commands
        private ICommand _checkForUniqueness;
        #endregion
        #endregion

        #region Properties
        #region Commands
        public ICommand CheckForUniqueness
        {
            get
            {
                return _checkForUniqueness ?? (_checkForUniqueness = new RelayCommand<object>((object o) =>
                {
                    foreach (Models.AlarmClock alarmClock in StationManager.CurrentUser.AlarmClocks)
                    {
                        if (alarmClock.NextTriggerDate == NewDateTime)
                        {
                            SystemSounds.Beep.Play();
                            MessageBox.Show("The alarm time must be unique.");
                            return;
                        }
                    }

                    OnNewAlarmClockUnique();
                }));
            }
        }
        #endregion

        public DateTime NewDateTime
        {
            get { return _newDateTime; }
            set
            {
                _newDateTime = value;
                OnPropertyChanged();
            }
        }

        public string SelectedHour
        {
            get { return _selectedHour; }
            set
            {
                _selectedHour = value;
                NewDateTime = NewDateTime.AddHours(Int32.Parse(SelectedHour));
                OnPropertyChanged();
            }
        }

        public string SelectedMinute
        {
            get { return _selectedMinute; }
            set
            {
                _selectedMinute = value;
                NewDateTime = NewDateTime.AddMinutes(Int32.Parse(SelectedMinute));
                OnPropertyChanged();
            }
        }

        public List<string> Hours
        {
            get { return _hours; }
            set { _hours = value; }
        }

        public List<string> Minutes
        {
            get { return _minutes; }
            set { _minutes = value; }
        }
        #endregion

        #region Constructor

        public CreateAlarmClockViewModel()
        {
            Initialize();
        }

        #endregion

        private void Initialize()
        {
            Hours = TimeGenerator.GenerateHours();
            Minutes = TimeGenerator.GenerateMinutes();
            SelectedHour = TimeGenerator.GetFormattedTime(0);
            SelectedMinute = TimeGenerator.GetFormattedTime(0);
        }

        #region EventsAndHandlers
        
        internal event newAlarmClockUniqueHandler NewAlarmClockUnique;
        internal delegate void newAlarmClockUniqueHandler();

        internal virtual void OnNewAlarmClockUnique()
        {
            NewAlarmClockUnique?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
