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
using AlarmClock.Models;
using AlarmClock.ViewModels.AlarmClocks;
using AlarmClock.ViewModels.AlarmClocks.AlarmClock;
using AlarmClock.Views.AlarmClocks.AlarmClockConfig;

namespace AlarmClock.Views.AlarmClocks
{
    /// <summary>
    /// Interaction logic for AlarmClocksView.xaml
    /// </summary>
    public partial class AlarmClocksView : UserControl
    {
        private AlarmClocksViewModel _alarmClocksViewModel;

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
                AlarmClockView.Visibility = Visibility.Collapsed;
                return;
            }
            if (AlarmClockView == null)
            {
                AlarmClockView = new AlarmClockView();
            }
            AlarmClockViewModel alarmClockViewModel = new AlarmClockViewModel(alarmClock);
            AlarmClockView.Visibility = Visibility.Visible;
            AlarmClockView.DataContext = alarmClockViewModel;
            alarmClockViewModel.AlarmClockTimeUpdated += OnAlarmClockTimeUpdated;
        }

        private void OnAlarmClockTimeUpdated(Models.AlarmClock updatedAlarmClock)
        {
            alarmClocksList.Items.Refresh();
        }
    }
}
