using AlarmClock.Managers;
using System.Windows;

namespace AlarmClock
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            StationManager.ImportData();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            if(StationManager.CurrentUser!= null)
                StationManager.CurrentUser.AlarmClocks.ForEach(AlarmClock =>
                {
                    AlarmClock.IsAlarming = false;
                });
            StationManager.CloseApp();
        }
    }
}
