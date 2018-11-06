using AlarmClock.ViewModels.Authentication;

namespace AlarmClock.Views.Authentication
{
    /// <summary>
    /// Interaction logic for SignUpView.xaml
    /// </summary>
    internal partial class SignUpView
    {
        #region Constructor
        internal SignUpView()
        {
            InitializeComponent();
            var signUpViewModel = new SignUpViewModel();
            DataContext = signUpViewModel;
        }
        #endregion
    }
}
