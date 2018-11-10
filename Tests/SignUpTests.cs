using AlarmClock.ViewModels.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class SignUpTests
    {
        [TestMethod]
        public void TestSignUpCanExecute()
        {
            SignUpViewModel signInViewModel = new SignUpViewModel();
            PrivateObject obj = new PrivateObject(signInViewModel);
            SetData(signInViewModel, "login", "1234", "fn", "ln", "email@email.com");
            Assert.IsTrue((bool)obj.Invoke("SignUpCanExecute", (object)null));
            SetData(signInViewModel, " login ", " 1234  ", " fn ", " ln ", " email@email.com ");
            Assert.IsTrue((bool)obj.Invoke("SignUpCanExecute", (object)null));
            SetData(signInViewModel, "login", "", "", "ln", "");
            Assert.IsFalse((bool)obj.Invoke("SignUpCanExecute", (object)null));
            SetData(signInViewModel, "", "", "", "", "");
            Assert.IsFalse((bool)obj.Invoke("SignUpCanExecute", (object)null));
            SetData(signInViewModel, null, null, null, null, null);
            Assert.IsFalse((bool)obj.Invoke("SignUpCanExecute", (object)null));
        }

        private void SetData(SignUpViewModel signInViewModel, string login, string password, string firstName, string lastName, string email)
        {
            signInViewModel.Login = login;
            signInViewModel.Password = password;
            signInViewModel.FirstName = firstName;
            signInViewModel.LastName = lastName;
            signInViewModel.Email = email;
        }
    }
}
