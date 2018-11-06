using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AlarmClock.ViewModels.AlarmClocks;
using AlarmClock.ViewModels.AlarmClocks.AlarmClock;
using AlarmClock.Views.AlarmClocks.AlarmClock;

namespace AlarmClock.Views.AlarmClocks
{
    /// <summary>
    /// Interaction logic for AlarmClocksView.xaml
    /// </summary>
    public partial class AlarmClocksView : UserControl
    {
        private AlarmClocksViewModel _alarmClocksViewModel;
        private AlarmClockView _currentAlarmClockView;

        public AlarmClocksView()
        {
            InitializeComponent();
            Visibility = Visibility.Visible;
            _alarmClocksViewModel = new AlarmClocksViewModel();
            _alarmClocksViewModel.AlarmClockChanged += OnAlarmClockChanged;
            DataContext = _alarmClocksViewModel;
        }

        private void OnAlarmClockChanged(Models.AlarmClock alarmClock)
        {
            if(alarmClock == null)
            {
                _currentAlarmClockView.Visibility = Visibility.Collapsed;
                return;
            }
            if (_currentAlarmClockView == null)
            {
                _currentAlarmClockView = new AlarmClockView(alarmClock);
                AlarmClocksGrid.Children.Add(_currentAlarmClockView);
                Grid.SetRow(_currentAlarmClockView, 0);
                Grid.SetRowSpan(_currentAlarmClockView, 2);
                Grid.SetColumn(_currentAlarmClockView, 1);
            }
            AlarmClockViewModel alarmClockViewModel = new AlarmClockViewModel(alarmClock);
            _currentAlarmClockView.Visibility = Visibility.Visible;
            _currentAlarmClockView.DataContext = alarmClockViewModel;
            alarmClockViewModel.AlarmClockTimeUpdated += OnAlarmClockTimeUpdated;
        }

        private void OnAlarmClockTimeUpdated(Models.AlarmClock updatedAlarmClock)
        {
            alarmClocksList.Items.Refresh();
        }
    }
}
