using AlarmClock.ViewModels.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class SignInTests
    {
        [TestMethod]
        public void TestSignInCanExecute()
        {
            SignInViewModel signInViewModel = new SignInViewModel();
            signInViewModel.Login = "login";
            signInViewModel.Password = "1234";
            PrivateObject obj = new PrivateObject(signInViewModel);
            Assert.IsTrue((bool)obj.Invoke("SignInCanExecute", (object)null));
            signInViewModel.Login = " login ";
            signInViewModel.Password = "    1234 ";
            Assert.IsTrue((bool)obj.Invoke("SignInCanExecute", (object)null));
            signInViewModel.Login = " ";
            signInViewModel.Password = "1234";
            Assert.IsFalse((bool)obj.Invoke("SignInCanExecute", (object)null));
            signInViewModel.Login = "";
            signInViewModel.Password = "1234";
            Assert.IsFalse((bool)obj.Invoke("SignInCanExecute", (object)null));
            signInViewModel.Login = "login";
            signInViewModel.Password = "";
            Assert.IsFalse((bool)obj.Invoke("SignInCanExecute", (object)null));
            signInViewModel.Login = "";
            signInViewModel.Password = "";
            Assert.IsFalse((bool)obj.Invoke("SignInCanExecute", (object)null));
            signInViewModel.Login = "    ";
            signInViewModel.Password = "    ";
            Assert.IsFalse((bool)obj.Invoke("SignInCanExecute", (object)null));
            signInViewModel.Login = "  1  ";
            signInViewModel.Password = "";
            Assert.IsFalse((bool)obj.Invoke("SignInCanExecute", (object)null));
            signInViewModel.Login = "";
            signInViewModel.Password = "  1  ";
            Assert.IsFalse((bool)obj.Invoke("SignInCanExecute", (object)null));
        }
    }
}
