using Caliburn.Micro;
using Core.Types;
using System;
using System.Windows;
using WebClient.Interfaces;

namespace PacientRegistry.ViewModels
{
    public class UserLoginViewModel : Screen
    {
        private readonly IWebClient WebClient;

        private BindableCollection<User> _users = new BindableCollection<User>();
        private User _selectedUser;
        private string password;

        private Visibility _isWrongPassword = Visibility.Hidden;

        public Visibility IsWrongPassword
        {
            get { return _isWrongPassword; }
            set { _isWrongPassword = value; NotifyOfPropertyChange(() => IsWrongPassword); }
        }

        public UserLoginViewModel(IWebClient webClient)
        {
            WebClient = webClient;
        }

        public BindableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; NotifyOfPropertyChange(() => Users); }
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                NotifyOfPropertyChange(() => SelectedUser);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool CanLogin
        {
            get
            {
                if (password != null && SelectedUser != null)
                    return true;
                else return false;
            }
        }

        protected override async void OnActivate()
        {
            var r = await WebClient.GetUsersAsync();

            if (r != null)
            {
                Users.Clear();
                Users.AddRange(r);
            }

            base.OnActivate();
        }

        public async void Login()
        {
            try
            {
                IsWrongPassword = Visibility.Hidden;

                var ok = await WebClient.GetProgrammTokenAsync(SelectedUser, Password);

                if (ok)
                {
                    TryClose(true);
                }
            }
            catch (UnauthorizedAccessException)
            {
                IsWrongPassword = Visibility.Visible;
            }
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}