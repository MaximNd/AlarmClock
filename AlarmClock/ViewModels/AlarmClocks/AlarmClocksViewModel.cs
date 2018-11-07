﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AlarmClock.Managers;
using AlarmClock.Models;
using AlarmClock.Properties;
using AlarmClock.Tools;
using AlarmClock.Views.AlarmClocks;

namespace AlarmClock.ViewModels.AlarmClocks
{
    class AlarmClocksViewModel : INotifyPropertyChanged
    {
        #region Fields
        private AlarmClockForView _selectedAlarmClock;
        private ObservableCollection<AlarmClockForView> _alarmClocks = new ObservableCollection<AlarmClockForView>();
        private bool _isAlarmClockSelected = false;
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
                    CreateAlarmClockViewModel createAlarmClockViewModel = new CreateAlarmClockViewModel();
                    CreateAlarmClockView createAlarmClockView = new CreateAlarmClockView(createAlarmClockViewModel);

                    if (createAlarmClockView.ShowDialog() == true)
                    {
                        AlarmClockForView alarmClock = new AlarmClockForView(null, createAlarmClockViewModel.NewDateTime);
                        StationManager.CurrentUser.AlarmClocks.Add(alarmClock.AlarmClock);
                        AlarmClocks.Add(alarmClock);
                        SelectedAlarmClock = alarmClock;
                        IsAlarmClockSelected = true;
                    }
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

                    if (AlarmClocks.Count != 0)
                    {
                        SelectedAlarmClock = AlarmClocks[newIndex];
                    }
                    else
                    {
                        SelectedAlarmClock = null;
                        IsAlarmClockSelected = false;
                    }
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
        public bool IsAlarmClockSelected
        {
            get
            {
                return _isAlarmClockSelected;
            }
            set
            {
                _isAlarmClockSelected = value;
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
