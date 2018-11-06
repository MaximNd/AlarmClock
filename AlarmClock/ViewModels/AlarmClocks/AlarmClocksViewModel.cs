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
using AlarmClock.Models;
using AlarmClock.Properties;
using AlarmClock.Tools;

namespace AlarmClock.ViewModels.AlarmClocks
{
    class AlarmClocksViewModel : INotifyPropertyChanged
    {
        #region Fields
        private AlarmClockForView _selectedAlarmClock;
        private ObservableCollection<AlarmClockForView> _alarmClocks = new ObservableCollection<AlarmClockForView>();
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
                    AlarmClockForView alarmClock = new AlarmClockForView(null, new DateTime(today.Year, today.Month, today.Day+1, 0, 0, 0));
                    StationManager.CurrentUser.AlarmClocks.Add(alarmClock.AlarmClock);
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
                    int deletedAlarmClockIndex = AlarmClocks.IndexOf(SelectedAlarmClock);
                    int newIndex = deletedAlarmClockIndex == 0 ? deletedAlarmClockIndex : deletedAlarmClockIndex - 1;
                    AlarmClocks.Remove(SelectedAlarmClock);
                    SelectedAlarmClock = AlarmClocks.Count != 0 ? AlarmClocks[newIndex] : null;
                }));
            }
        }

        #endregion

        public ObservableCollection<AlarmClockForView> AlarmClocks
        {
            get { return _alarmClocks; }
        }
        public AlarmClockForView SelectedAlarmClock
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
            _alarmClocks = new ObservableCollection<AlarmClockForView>();
            foreach (Models.AlarmClock alarmClock in StationManager.CurrentUser.AlarmClocks)
            {
                _alarmClocks.Add(new AlarmClockForView(alarmClock));
            }
            if (_alarmClocks.Count > 0)
            {
                _selectedAlarmClock = AlarmClocks[0];
            }
        }

        #region EventsAndHandlers
        #region Loader
        internal event AlarmClockChangedHandler AlarmClockChanged;
        internal delegate void AlarmClockChangedHandler(AlarmClockForView alarmClock);

        internal virtual void OnAlarmClockChanged(AlarmClockForView alarmClock)
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
