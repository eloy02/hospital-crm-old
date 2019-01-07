using Caliburn.Micro;
using Core.Types;
using Core.Types.Enumerations;
using RehabilitationCentre.Models;
using RehabilitationCentre.ViewModels.ViewTypes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RehabilitationCentre.ViewModels
{
    public class AdminPageViewModel : Screen
    {
        private readonly AdminModel Model;

        public static string Name
        {
            get
            {
                return "Админка";
            }
        }

        private User _selectedUser;
        private Doctor _selectedDoctor;

        private BindableCollection<User> _users = new BindableCollection<User>();
        private BindableCollection<Doctor> _doctors = new BindableCollection<Doctor>();
        private BindableCollection<IsActiveView> _isActiveCollection = new BindableCollection<IsActiveView>();
        private BindableCollection<AccessGroupView> _accessGroups = new BindableCollection<AccessGroupView>();

        public BindableCollection<AccessGroupView> AccessGroups
        {
            get { return _accessGroups; }

            set
            {
                _accessGroups = value; NotifyOfPropertyChange(() => AccessGroups);
            }
        }

        public BindableCollection<IsActiveView> IsActiveCollection
        {
            get { return _isActiveCollection; }
            set { _isActiveCollection = value; NotifyOfPropertyChange(() => IsActiveCollection); }
        }

        public AdminPageViewModel(AdminModel model)
        {
            Model = model;
        }

        public BindableCollection<Doctor> Doctors
        {
            get { return _doctors; }
            set { _doctors = value; NotifyOfPropertyChange(() => Doctors); }
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
                _selectedUser = value; NotifyOfPropertyChange(() => SelectedUser);
                NotifyOfPropertyChange(() => CanSaveUser);
            }
        }

        public Doctor SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value; NotifyOfPropertyChange(() => SelectedDoctor);
                NotifyOfPropertyChange(() => CanSaveDoctor);
            }
        }

        public bool CanSaveDoctor
        {
            get
            {
                return !(_selectedDoctor is null);
            }
        }

        public bool CanSaveUser
        {
            get
            {
                return !(_selectedUser is null);
            }
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();

            var t1 = Task.Run(async () =>
            {
                var r = await Model.GetDoctorsAsync();

                Doctors = new BindableCollection<Doctor>(r);
            });

            var t2 = Task.Run(async () =>
            {
                var r = await Model.GetUsersAsync();

                Users = new BindableCollection<User>(r);
            });

            await Task.WhenAll(t1, t2);

            IsActiveCollection = new BindableCollection<IsActiveView>()
            {
                new IsActiveView
                {
                     IsActive = true,
                     Name = "Доступно"
                },
                new IsActiveView
                {
                    IsActive = false,
                     Name = "Недоступно"
                }
            };

            var accgroups = Enum.GetValues(typeof(EAccessGroup)).Cast<EAccessGroup>();
            AccessGroups = new BindableCollection<AccessGroupView>(accgroups.Select(a => new AccessGroupView()
            {
                AccessGroup = a,
                Name = a.GetDescription()
            }));
        }

        public async Task SaveDoctor()
        {
            if (SelectedDoctor != null)
            {
                var doc = await Model.UpdateDoctorAsync(SelectedDoctor);

                if (doc != null)
                {
                    var pos = Doctors.IndexOf(Doctors.SingleOrDefault(d => d.Id == doc.Id));
                    Doctors[pos] = doc;
                }
            }
        }

        public async Task SaveUser()
        {
            if (SelectedUser != null)
            {
                var us = await Model.UpdateUserAsync(SelectedUser);

                if (us != null)
                {
                    var pos = Users.IndexOf(Users.SingleOrDefault(u => u.Id == us.Id));
                    Users[pos] = us;
                }
            }
        }
    }
}