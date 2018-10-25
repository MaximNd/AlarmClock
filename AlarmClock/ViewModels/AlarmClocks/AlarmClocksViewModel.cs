using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using AlarmClock.Managers;
using AlarmClock.Properties;
using AlarmClock.Tools;

namespace AlarmClock.ViewModels.AlarmClocks
{
    class AlarmClocksViewModel : INotifyPropertyChanged
    {
        #region Fields
        private Models.AlarmClock _selectedAlarmClock;
        private ObservableCollection<Models.AlarmClock> _alarmClocks;
        #region Commands
        private ICommand _addAlarmClockCommand;
        private ICommand _deleteAlarmClockCommand;
        #endregion
        #endregion

        #region Properties
        #region Commands

        public ICommand AddAlarmClockCommand
        {
            get
            {
                return _addAlarmClockCommand ?? (_addAlarmClockCommand = new RelayCommand<object>((object o) =>
                {
                    DateTime today = DateTime.Today;
                    Models.AlarmClock alarmClock = new Models.AlarmClock(null, new DateTime(today.Year, today.Month, today.Day+1, 0, 0, 0));
                    StationManager.CurrentUser.AlarmClocks.Add(alarmClock);
                    AlarmClocks.Add(alarmClock);
                    SelectedAlarmClock = alarmClock;
                }));
            }
        }

        public ICommand DeleteAlarmClockCommand
        {
            get
            {
                return _deleteAlarmClockCommand ?? (_deleteAlarmClockCommand = new RelayCommand<object>((object o) =>
                {
                    if (SelectedAlarmClock == null) return;
                    
                    StationManager.CurrentUser.AlarmClocks.RemoveAll(alarmClock => alarmClock.Guid == SelectedAlarmClock.Guid);
                    FillAlarmClocks();
                    OnPropertyChanged(nameof(SelectedAlarmClock));
                    OnPropertyChanged(nameof(AlarmClocks));
                }));
            }
        }

        #endregion

        public ObservableCollection<Models.AlarmClock> AlarmClocks
        {
            get { return _alarmClocks; }
        }
        public Models.AlarmClock SelectedAlarmClock
        {
            get { return _selectedAlarmClock; }
            set
            {
                _selectedAlarmClock = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor
        public AlarmClocksViewModel()
        {
            FillAlarmClocks();
            PropertyChanged += OnPropertyChanged;
        }
        #endregion
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(SelectedAlarmClock) || propertyChangedEventArgs.PropertyName == nameof(_selectedAlarmClock))
                OnAlarmClockChanged(SelectedAlarmClock);
        }
        private void FillAlarmClocks()
        {
            _alarmClocks = new ObservableCollection<Models.AlarmClock>();
            foreach (Models.AlarmClock alarmClock in StationManager.CurrentUser.AlarmClocks)
            {
                _alarmClocks.Add(alarmClock);
            }
            if (_alarmClocks.Count > 0)
            {
                _selectedAlarmClock = AlarmClocks[0];
            }
        }

        #region EventsAndHandlers
        #region Loader
        internal event AlarmClockChangedHandler AlarmClockChanged;
        internal delegate void AlarmClockChangedHandler(Models.AlarmClock alarmClock);

        internal virtual void OnAlarmClockChanged(Models.AlarmClock alarmClock)
        {
            AlarmClockChanged?.Invoke(alarmClock);
        }
        #endregion
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion
    }
}
