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
using AlarmClock.ViewModels.AlarmClocks.AlarmClock;

namespace AlarmClock.Views.AlarmClocks.AlarmClock
{
    /// <summary>
    /// Interaction logic for AlarmClockView.xaml
    /// </summary>
    public partial class AlarmClockView : UserControl
    {
        public AlarmClockView(Models.AlarmClock alarmClock)
        {
            InitializeComponent();
            var alarmClockModel = new AlarmClockViewModel(alarmClock);
            DataContext = alarmClockModel;
        }
    }
}
