using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmClock.Views.Authentication;

namespace AlarmClock.Tools
{
    internal enum ModesEnum
    {
        SignIn
    }

    internal class NavigationModel
    {
        private readonly IContentWindow _contentWindow;
        private SignInView _signInView;

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

    }
}
