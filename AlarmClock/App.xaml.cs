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
            StationManager.CloseApp();
        }
    }
}
