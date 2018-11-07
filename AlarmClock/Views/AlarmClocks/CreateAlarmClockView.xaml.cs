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
using System.Windows.Shapes;
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
