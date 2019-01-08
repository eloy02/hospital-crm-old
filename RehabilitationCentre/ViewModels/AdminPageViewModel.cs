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
        private string _userFirstName;
        private string _userLastName;
        private string _userPatronymicName;
        private EAccessGroup? _userAccessGroup;
        private bool? _userActiveStatus;

        private string _doctorFirstName;
        private string _doctorLastName;
        private string _doctorPatronymicName;
        private string _doctorPosition;
        private bool? _doctorIsActiveStatus;
        private string _userPasswod;

        public string UserPassword
        {
            get { return _userPasswod; }
            set
            {
                _userPasswod = value;

                NotifyOfPropertyChange(() => CanCreateUser);
                NotifyOfPropertyChange(() => UserPassword);
            }
        }

        public bool? UserIsActiveStatus
        {
            get { return _userActiveStatus; }
            set { _userActiveStatus = value; NotifyOfPropertyChange(() => UserIsActiveStatus); }
        }

        public EAccessGroup? UserAccessGroup
        {
            get { return _userAccessGroup; }
            set { _userAccessGroup = value; NotifyOfPropertyChange(() => UserAccessGroup); }
        }

        public string UserPatronymicName
        {
            get { return _userPatronymicName; }
            set { _userPatronymicName = value; NotifyOfPropertyChange(() => UserPatronymicName); }
        }

        public string UserLastName
        {
            get { return _userLastName; }
            set { _userLastName = value; NotifyOfPropertyChange(() => UserLastName); }
        }

        public string UserFirstName
        {
            get { return _userFirstName; }
            set { _userFirstName = value; NotifyOfPropertyChange(() => UserFirstName); }
        }

        public string DoctorPosition
        {
            get { return _doctorPosition; }
            set { _doctorPosition = value; NotifyOfPropertyChange(() => DoctorPosition); }
        }

        public string DoctorPatronymicName
        {
            get { return _doctorPatronymicName; }
            set { _doctorPatronymicName = value; NotifyOfPropertyChange(() => DoctorPatronymicName); }
        }

        public string DoctorLastName
        {
            get { return _doctorLastName; }
            set
            {
                _doctorLastName = value;

                NotifyOfPropertyChange(() => DoctorLastName);
                NotifyOfPropertyChange(() => CanCreateDoctor);
            }
        }

        public string DoctorFirstName
        {
            get { return _doctorFirstName; }
            set
            {
                _doctorFirstName = value;

                NotifyOfPropertyChange(() => DoctorFirstName);
            }
        }

        public bool? DoctorIsActiveStatus
        {
            get { return _doctorIsActiveStatus; }
            set { _doctorIsActiveStatus = value; NotifyOfPropertyChange(() => DoctorIsActiveStatus); }
        }

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
                _selectedUser = value;

                UserAccessGroup = value?.Access;
                UserIsActiveStatus = value?.IsActive;
                UserFirstName = value?.FirstName;
                UserLastName = value?.LastName;
                UserPatronymicName = value?.PatronymicName;

                NotifyOfPropertyChange(() => SelectedUser);
                NotifyOfPropertyChange(() => CanSaveUser);
            }
        }

        public Doctor SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value;

                DoctorFirstName = value?.FirstName;
                DoctorIsActiveStatus = value?.IsActive;
                DoctorLastName = value?.LastName;
                DoctorPatronymicName = value?.PatronymicName;
                DoctorPosition = value?.Position;

                NotifyOfPropertyChange(() => SelectedDoctor);
                NotifyOfPropertyChange(() => CanSaveDoctor);
                NotifyOfPropertyChange(() => CanClearSelectedDoctor);
                NotifyOfPropertyChange(() => CanCreateDoctor);
            }
        }

        public bool CanCreateDoctor
        {
            get
            {
                if (SelectedDoctor is null && !string.IsNullOrEmpty(DoctorLastName))
                    return true;
                else return false;
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

        public bool CanClearSelectedDoctor
        {
            get
            {
                return !(SelectedDoctor is null);
            }
        }

        public bool CanCreateUser
        {
            get
            {
                if (SelectedUser is null && !string.IsNullOrEmpty(UserPassword) && !string.IsNullOrEmpty(UserLastName))
                    return true;
                else return false;
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
                SelectedDoctor.FirstName = DoctorFirstName;
                SelectedDoctor.LastName = DoctorLastName;
                SelectedDoctor.PatronymicName = DoctorPatronymicName;
                SelectedDoctor.Position = DoctorPosition;
                SelectedDoctor.IsActive = DoctorIsActiveStatus ?? true;

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
                SelectedUser.FirstName = UserFirstName;
                SelectedUser.LastName = UserLastName;
                SelectedUser.PatronymicName = UserPatronymicName;
                SelectedUser.IsActive = UserIsActiveStatus ?? true;
                SelectedUser.Access = UserAccessGroup ?? EAccessGroup.Admin;

                var us = await Model.UpdateUserAsync(SelectedUser);

                if (us != null)
                {
                    var pos = Users.IndexOf(Users.SingleOrDefault(u => u.Id == us.Id));
                    Users[pos] = us;
                }
            }
        }

        public async Task CreateDoctor()
        {
            var doc = new Doctor()
            {
                FirstName = DoctorFirstName,
                LastName = DoctorLastName,
                IsActive = DoctorIsActiveStatus ?? true,
                PatronymicName = DoctorPatronymicName,
                Position = DoctorPosition,
                Id = default
            };

            doc = await Model.CreateDoctorAsync(doc);

            if (doc != null)
            {
                Doctors.Add(doc);
                ClearSelectedDoctor();
            }
        }

        public async Task CreateUser()
        {
            var user = new User()
            {
                Access = UserAccessGroup ?? EAccessGroup.RegAndCentre,
                FirstName = UserFirstName,
                Id = default,
                IsActive = UserIsActiveStatus ?? true,
                LastName = UserLastName,
                PatronymicName = UserPatronymicName
            };

            user = await Model.CreateUserAsync(user, UserPassword);

            if (user != null)
            {
                Users.Add(user);
                ClearSeletedUser();
            }
        }

        public void ClearSelectedDoctor()
        {
            SelectedDoctor = null;

            DoctorFirstName = null;
            DoctorIsActiveStatus = null;
            DoctorLastName = null;
            DoctorPatronymicName = null;
            DoctorPosition = null;
        }

        public void ClearSeletedUser()
        {
            SelectedUser = null;

            UserAccessGroup = null;
            UserFirstName = null;
            UserIsActiveStatus = null;
            UserLastName = null;
            UserPassword = null;
            UserPatronymicName = null;
            UserAccessGroup = null;
        }

        public void ClearUserFields() => ClearSeletedUser();

        public void ClearDoctorFields() => ClearSelectedDoctor();
    }
}