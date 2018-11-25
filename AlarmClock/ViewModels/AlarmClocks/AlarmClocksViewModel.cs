using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmClock.Managers;
using AlarmClock.Models;
using AlarmClock.Properties;
using AlarmClock.Tools;
using AlarmClock.Views.AlarmClocks;
using System.Linq;
using Managers;
using Tools;

[assembly: InternalsVisibleTo("Tests")]

namespace AlarmClock.ViewModels.AlarmClocks
{
    class AlarmClocksViewModel : INotifyPropertyChanged
    {
        #region Fields
        private AlarmClockForView _selectedAlarmClock;
        private ObservableCollection<AlarmClockForView> _alarmClocks = new ObservableCollection<AlarmClockForView>();
        private bool _isAlarmClockSelected;
        #region Commands
        private ICommand _addAlarmClockCommand;
        private ICommand _deleteAlarmClockCommand;
        private ICommand _logoutCommand;
        #endregion
        #endregion

        #region Properties
        #region Commands

        public ICommand AddAlarmClockCommand
        {
            get
            {
                return _addAlarmClockCommand ?? (_addAlarmClockCommand = new RelayCommand<object>(async (object o) =>
                {
                    CreateAlarmClockViewModel createAlarmClockViewModel = new CreateAlarmClockViewModel();
                    CreateAlarmClockView createAlarmClockView = new CreateAlarmClockView(createAlarmClockViewModel);

                    if (createAlarmClockView.ShowDialog() == true)
                    {
                        LoaderManager.Instance.ShowLoader();
                        
                        AlarmClockForView alarmClock = new AlarmClockForView(null, createAlarmClockViewModel.NewDateTime);
                        await AddNewAlarmClock(alarmClock);
                        LoaderManager.Instance.HideLoader();
                        Logger.Log($"User: {StationManager.CurrentUser} create new AlarmClock: {alarmClock.AlarmClock}");
                    }
                }));
            }
        }

        public ICommand DeleteAlarmClockCommand
        {
            get
            {
                return _deleteAlarmClockCommand ?? (_deleteAlarmClockCommand = new RelayCommand<object>(async (object o) =>
                {
                    if (SelectedAlarmClock == null) return;
                    LoaderManager.Instance.ShowLoader();
                    
                    DBModels.AlarmClock alarmClockToDelete = SelectedAlarmClock.AlarmClock;
                    await DeleteSelectedAlarmClock();
                    Logger.Log($"User: {StationManager.CurrentUser} delete AlarmClock: {alarmClockToDelete}");
                    LoaderManager.Instance.HideLoader();
                }));
            }
        }

        public ICommand LogoutCommand
        {
            get
            {
                return _logoutCommand ?? (_logoutCommand = new RelayCommand<object>(LogoutExecute));
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
        public AlarmClocksViewModel(AlarmClockChangedHandler onAlarmClockChanged)
        {
            AlarmClockChanged += onAlarmClockChanged;
            PropertyChanged += OnPropertyChanged;
            FillAlarmClocks();
            IsAlarmClockSelected = SelectedAlarmClock != null;

            CheckAlarms();
            
        }
        #endregion
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(SelectedAlarmClock) ||
                propertyChangedEventArgs.PropertyName == nameof(CheckAlarms) ||
                propertyChangedEventArgs.PropertyName == nameof(_selectedAlarmClock))
            {
                OnAlarmClockChanged(SelectedAlarmClock);
                IsAlarmClockSelected = SelectedAlarmClock != null;
            }
        }

        public void FillAlarmClocks()
        {
            _alarmClocks = new ObservableCollection<AlarmClockForView>();
            foreach (DBModels.AlarmClock alarmClock in StationManager.CurrentUser.AlarmClocks)
            {
                _alarmClocks.Add(new AlarmClockForView(alarmClock));
            }
            if (_alarmClocks.Count > 0)
            {
                SelectedAlarmClock = AlarmClocks[0];
                OnAlarmClockChanged(SelectedAlarmClock);
            }
        }

        private async void CheckAlarms()
        {
            while (true)
            {
                AlarmClockForView alarmClockForView = await Task.Run(() =>
                {
                    while (true)
                    {
                        DateTime nowStart = DateTime.Now;
                        foreach (AlarmClockForView afc in AlarmClocks)
                        {
                            if (!afc.IsAlarming && nowStart > afc.AlarmClock.NextTriggerDate)
                            {
                                return afc;
                            }
                            Console.WriteLine(nowStart - afc.AlarmClock.NextTriggerDate);
                        }
                        Thread.Sleep(500);
                    }
                });
                alarmClockForView.Alarm();
                SelectedAlarmClock = alarmClockForView;
                DBManager.SaveAlarmClock(SelectedAlarmClock.AlarmClock);
            }
        }

        private async Task AddNewAlarmClock(AlarmClockForView alarmClock)
        {
            alarmClock.AlarmClock.UserGuid = StationManager.CurrentUser.Guid;
            await Task.Run(() =>
            {
                DBManager.AddAlarmClock(alarmClock.AlarmClock);
            });
            AlarmClocks.Add(alarmClock);
            StationManager.CurrentUser.AlarmClocks.Add(alarmClock.AlarmClock);
            SelectedAlarmClock = alarmClock;
            IsAlarmClockSelected = true;
        }

        private async Task DeleteSelectedAlarmClock()
        {
            DBModels.AlarmClock alarmClockToDelete = StationManager.CurrentUser.AlarmClocks.FirstOrDefault(alarmClock => alarmClock.Guid == SelectedAlarmClock.AlarmClock.Guid);
            await Task.Run(() =>
            {
                DBManager.DeleteAlarmClock(alarmClockToDelete);
            });
            StationManager.CurrentUser.AlarmClocks.RemoveAll(alarmClock => alarmClock.Guid == SelectedAlarmClock.AlarmClock.Guid);
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
        }

        private void LogoutExecute(object obj)
        {
            Logger.Log($"User: {StationManager.CurrentUser} is logged out");
            SelectedAlarmClock = null;
            StationManager.CurrentUser = null;
            AlarmClocks.Clear();
            NavigationManager.Instance.Navigate(ModesEnum.SignIn);
        }

        #region EventsAndHandlers
        #region Loader
        internal event AlarmClockChangedHandler AlarmClockChanged;
        internal delegate void AlarmClockChangedHandler(AlarmClockForView alarmClock);
        internal void AlarmingHandler(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged();
        }

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
