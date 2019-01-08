using Caliburn.Micro;
using Core.Types;
using RehabilitationCentre.Views;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WebClient.Interfaces;

namespace RehabilitationCentre.ViewModels
{
    public class ShellViewModel : Conductor<object>, IShell
    {
        private readonly IWebClient WebClient;

        private readonly PacientsListViewModel PacientsListViewModel;
        private readonly ReportsViewModel ReportsViewModel;
        private readonly PacientRegistryViewModel PacientRegistryViewModel;
        private readonly AdminPageViewModel AdminPageViewModel;

        private UserLoginView UserLoginDialog = new UserLoginView();

        private Visibility _isWrongPassword = Visibility.Hidden;

        private User _selectedUser;

        private BindableCollection<object> _menuItems = new BindableCollection<object>();
        private BindableCollection<User> _users = new BindableCollection<User>();

        private object _currentDialogView;
        private object _currentView;

        private string password;
        private bool _isDialogShown;

        public ShellViewModel(IWebClient webClient, PacientsListViewModel pacientsListViewModel, ReportsViewModel reportsViewModel, PacientRegistryViewModel pacientRegistryViewModel, AdminPageViewModel adminPageViewModel)
        {
            WebClient = webClient;
            PacientsListViewModel = pacientsListViewModel;
            ReportsViewModel = reportsViewModel;
            PacientRegistryViewModel = pacientRegistryViewModel;
            AdminPageViewModel = adminPageViewModel;

            Application.Current.Exit += CurrentApp_Exit;
        }

        private void CurrentApp_Exit(object sender, ExitEventArgs e)
        {
            var path = Directory.GetCurrentDirectory() + @"\Temp";
            DirectoryInfo di = new DirectoryInfo(path);

            if (di.Exists)
            {
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }
            }

            DirectoryInfo di2 = new DirectoryInfo(@".\CompletedReports");

            if (di2.Exists)
            {
                foreach (FileInfo file in di2.EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di2.EnumerateDirectories())
                {
                    dir.Delete(true);
                }
            }

            Task.Run(async () => await WebClient.DeleteToken()).Wait();
        }

        public object CurrentDialogView
        {
            get { return _currentDialogView; }
            set { _currentDialogView = value; NotifyOfPropertyChange(() => CurrentDialogView); }
        }

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; NotifyOfPropertyChange(() => CurrentView); OpenNewView(); }
        }

        public bool IsDialogShown
        {
            get { return _isDialogShown; }
            set { _isDialogShown = value; NotifyOfPropertyChange(() => IsDialogShown); }
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

        public Visibility IsWrongPassword
        {
            get { return _isWrongPassword; }
            set { _isWrongPassword = value; NotifyOfPropertyChange(() => IsWrongPassword); }
        }

        public BindableCollection<object> MenuItems
        {
            get { return _menuItems; }
            set { _menuItems = value; NotifyOfPropertyChange(() => MenuItems); }
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

        public BindableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; NotifyOfPropertyChange(() => Users); }
        }

        public void Cancel()
        {
            Application.Current.Shutdown();
        }

        public void OpenNewView()
        {
            if (CurrentView != null)
            {
                ActivateItem(CurrentView);
            }
        }

        public async void Login()
        {
            try
            {
                IsWrongPassword = Visibility.Hidden;

                var ok = await WebClient.GetProgrammTokenAsync(SelectedUser, Password);

                if (ok.requestResult)
                {
                    IsDialogShown = false;

                    switch (ok.accessGroup)
                    {
                        case Core.Types.Enumerations.EAccessGroup.Admin:
                            {
                                MenuItems.Add(PacientsListViewModel);
                                MenuItems.Add(ReportsViewModel);
                                MenuItems.Add(PacientRegistryViewModel);
                                MenuItems.Add(AdminPageViewModel);

                                break;
                            }
                        case Core.Types.Enumerations.EAccessGroup.RehabilitationCentre:
                            {
                                MenuItems.Add(PacientsListViewModel);
                                MenuItems.Add(ReportsViewModel);

                                break;
                            }

                        case Core.Types.Enumerations.EAccessGroup.RegPacients:
                            {
                                MenuItems.Add(PacientRegistryViewModel);

                                break;
                            }
                        case Core.Types.Enumerations.EAccessGroup.RegAndCentre:
                            {
                                MenuItems.Add(PacientsListViewModel);
                                MenuItems.Add(ReportsViewModel);
                                MenuItems.Add(PacientRegistryViewModel);

                                break;
                            }
                    }
                }

                CurrentView = PacientsListViewModel;
            }
            catch (UnauthorizedAccessException)
            {
                IsWrongPassword = Visibility.Visible;
            }
        }

        protected override async void OnInitialize()
        {
            CurrentDialogView = UserLoginDialog;
            IsDialogShown = true;

            var r = await WebClient.GetUsersAsync();

            if (r != null)
            {
                Users.Clear();
                Users.AddRange(r.Where(u => u.IsActive));
            }

            base.OnInitialize();
        }
    }
}