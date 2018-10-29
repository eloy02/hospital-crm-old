using Caliburn.Micro;

namespace RehabilitationCentre.ViewModels
{
    public class UserLoginViewModel : Screen
    {
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyOfPropertyChange(() => Password); }
        }
    }
}