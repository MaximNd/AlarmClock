using System.Windows;
using AlarmClock.ViewModels.AlarmClocks;

namespace AlarmClock.Views.AlarmClocks
{
    /// <summary>
    /// Interaction logic for CreateAlarmClockView.xaml
    /// </summary>
    public partial class CreateAlarmClockView : Window
    {
        internal CreateAlarmClockView(CreateAlarmClockViewModel createAlarmClockViewModel)
        {
            InitializeComponent();
            createAlarmClockViewModel.NewAlarmClockUnique += OnNewAlarmClockUnique;
            DataContext = createAlarmClockViewModel;
        }

        private void OnNewAlarmClockUnique()
        {
            DialogResult = true;
        }
    }
}
