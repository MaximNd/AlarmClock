using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AlarmClock.Managers;
using AlarmClock.Models;
using AlarmClock.Properties;
using AlarmClock.Tools;
[assembly: InternalsVisibleTo("Tests")]

namespace AlarmClock.ViewModels.Authentication
{
    internal class SignUpViewModel : INotifyPropertyChanged
    {
        #region Fields
        private string _password;
        private string _login;
        private string _firstName;
        private string _lastName;
        private string _email;
        #region Commands
        private ICommand _closeCommand;
        private ICommand _signUpCommand;
        private ICommand _signInCommand;
        #endregion
        #endregion

        #region Properties
        #region Command

        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommand<object>(CloseExecute));
            }
        }

        public ICommand SignUpCommand
        {
            get
            {
                return _signUpCommand ?? (_signUpCommand = new RelayCommand<object>(SignUpExecute, SignUpCanExecute));
            }
        }
        public ICommand SignInCommand
        {
            get
            {
                return _signInCommand ?? (_signInCommand = new RelayCommand<object>(SignInExecute));
            }
        }
        #endregion

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ConstructorAndInit
        internal SignUpViewModel()
        {
        }
        #endregion

        private async void SignUpExecute(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            var res = await Task.Run(() =>
            {
                // TODO delete this later
                // faking delay
                Thread.Sleep(500);

                try
                {
                    if (!new EmailAddressAttribute().IsValid(_email))
                    {
                        string message = $"{Resources.SignUp_EmailIsNotValid} {_email}";
                        MessageBox.Show(message);
                        Logger.Log($"{message}");
                        return false;
                    }
                    if (DBManager.UserExists(_login))
                    {
                        string message = $"{Resources.SignUp_UserAlreadyExists} {_login}";
                        MessageBox.Show(String.Format(Resources.SignUp_UserAlreadyExists, _login));
                        Logger.Log($"{message}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(Resources.SignUp_FailedToValidateData, Environment.NewLine,
                        ex.Message));
                    Logger.Log($"{Resources.SignUp_FailedToValidateData}", ex);
                    return false;
                }
                try
                {
                    var user = new User(_firstName, _lastName, _email, _login, _password);
                    DBManager.AddUser(user);
                    StationManager.CurrentUser = user;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(Resources.SignUp_FailedToCreateUser, Environment.NewLine,
                        ex.Message));
                    return false;
                }
                MessageBox.Show(String.Format(Resources.SignUp_UserSuccessfulyCreated, _login));
                Logger.Log($"{Resources.SignUp_UserSuccessfulyCreated}, {_login}");
                return true;
            });
            LoaderManager.Instance.HideLoader();
            if (res)
            {
                NavigationManager.Instance.Navigate(ModesEnum.AlarmsClocks);
            }
        }
        private bool SignUpCanExecute(object obj)
        {
            return !String.IsNullOrEmpty(_login) &&
                   !String.IsNullOrEmpty(_password) &&
                   !String.IsNullOrEmpty(_firstName) &&
                   !String.IsNullOrEmpty(_lastName) &&
                   !String.IsNullOrEmpty(_email);
        }
        private void SignInExecute(object obj)
        {
            NavigationManager.Instance.Navigate(ModesEnum.SignIn);
        }
        private void CloseExecute(object obj)
        {
            StationManager.CloseApp();
        }

        #region EventsAndHandlers
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion
    }
}
