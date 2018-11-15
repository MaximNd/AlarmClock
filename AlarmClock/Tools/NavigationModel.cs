using System;
using AlarmClock.Views.AlarmClocks;
using AlarmClock.Views.Authentication;

namespace AlarmClock.Tools
{
    internal enum ModesEnum
    {
        SignIn,
        SingUp,
        AlarmsClocks
    }

    internal class NavigationModel
    {
        private readonly IContentWindow _contentWindow;
        private SignInView _signInView;
        private SignUpView _signUpView;
        private AlarmClocksView _alarmsClocksView;

        internal NavigationModel(IContentWindow contentWindow)
        {
            _contentWindow = contentWindow;
        }

        internal void Navigate(ModesEnum mode)
        {
            switch (mode)
            {
                case ModesEnum.SignIn:
                    _contentWindow.ContentControl.Content = _signInView ?? (_signInView = new SignInView());
                    break;
                case ModesEnum.SingUp:
                    _contentWindow.ContentControl.Content = _signUpView ?? (_signUpView = new SignUpView());
                    break;
                case ModesEnum.AlarmsClocks:
                    _contentWindow.ContentControl.Content = _alarmsClocksView = new AlarmClocksView();
                    break;;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

    }
}
