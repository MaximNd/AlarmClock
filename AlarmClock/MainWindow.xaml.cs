using System.Windows.Controls;
using AlarmClock.Managers;
using AlarmClock.Tools;

namespace AlarmClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IContentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var navigationModel = new NavigationModel(this);
            NavigationManager.Instance.Initialize(navigationModel);
            if(StationManager.CurrentUser==null)
                navigationModel.Navigate(ModesEnum.SignIn);
            else
                navigationModel.Navigate(ModesEnum.AlarmsClocks);
        }

        public ContentControl ContentControl
        {
            get { return _contentControl; }
        }
    }
}
