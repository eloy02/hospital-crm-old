using Caliburn.Micro;
using Core.Types;
using Core.Types.Enumerations;
using MaterialDesignThemes.Wpf;
using RehabilitationCentre.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RehabilitationCentre.ViewModels
{
    public class PacientsListViewModel : Conductor<object>, IShell
    {
        #region Private fields

        private bool _chooseDoctor = false;
        private string _firstNameForFilter;
        private bool _isPacientsLoading = false;
        private string _lastNameForFilter;
        private string _pacientPhoneNumberForFilter;
        private PacientTypeView _pacientTypeForFilter;
        private BindableCollection<PacientTypeView> _pacientTypes = new BindableCollection<PacientTypeView>();
        private string _parentPhoneNumberForFilter;
        private string _patronymicNameForFilter;
        private Doctor _selectedDoctor;
        private Pacient _selectedPacient;

        private BindableCollection<Pacient> pacients = new BindableCollection<Pacient>();
        private DispatcherTimer Timer;
        private BindableCollection<Doctor> _doctors = new BindableCollection<Doctor>();

        #endregion Private fields

        #region Public properties

        public static object Icon
        {
            get { return PackIconKind.ViewList; }
        }

        public static string Name
        {
            get { return "Список пациентов"; }
        }

        public bool ChooseDoctor
        {
            get { return _chooseDoctor; }
            set { _chooseDoctor = value; NotifyOfPropertyChange(() => ChooseDoctor); }
        }

        public string FirstNameForFilter
        {
            get { return _firstNameForFilter; }
            set
            {
                _firstNameForFilter = value;
                NotifyOfPropertyChange(() => FirstNameForFilter);
                NotifyOfPropertyChange(() => CanClearFilter);
                FilterPacients();
            }
        }

        public bool IsPacientsLoading
        {
            get { return _isPacientsLoading; }
            set { _isPacientsLoading = value; NotifyOfPropertyChange(() => IsPacientsLoading); }
        }

        public string LastNameForFilter
        {
            get { return _lastNameForFilter; }
            set
            {
                _lastNameForFilter = value;
                NotifyOfPropertyChange(() => LastNameForFilter);
                NotifyOfPropertyChange(() => CanClearFilter);
                FilterPacients();
            }
        }

        public string PacientPhoneNumberForFilter
        {
            get { return _pacientPhoneNumberForFilter; }
            set
            {
                _pacientPhoneNumberForFilter = value;
                NotifyOfPropertyChange(() => PacientPhoneNumberForFilter);
                NotifyOfPropertyChange(() => CanClearFilter);
                FilterPacients();
            }
        }

        public PacientTypeView PacientTypeForFilter
        {
            get { return _pacientTypeForFilter; }
            set
            {
                _pacientTypeForFilter = value;
                NotifyOfPropertyChange(() => PacientTypeForFilter);
                NotifyOfPropertyChange(() => CanClearFilter);
                FilterPacients();
            }
        }

        public string ParentPhoneNumberForFilter
        {
            get { return _parentPhoneNumberForFilter; }
            set
            {
                _parentPhoneNumberForFilter = value;
                NotifyOfPropertyChange(() => ParentPhoneNumberForFilter);
                NotifyOfPropertyChange(() => CanClearFilter);
                FilterPacients();
            }
        }

        public string PatronymicNameForFilter
        {
            get { return _patronymicNameForFilter; }
            set
            {
                _patronymicNameForFilter = value;
                NotifyOfPropertyChange(() => PatronymicNameForFilter);
                NotifyOfPropertyChange(() => CanClearFilter);
                FilterPacients();
            }
        }

        public Doctor SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value;
                NotifyOfPropertyChange(() => SelectedDoctor);
                NotifyOfPropertyChange(() => CanSetVisit);
            }
        }

        public Pacient SelectedPacient
        {
            get { return _selectedPacient; }
            set { _selectedPacient = value; NotifyOfPropertyChange(() => SelectedPacient); }
        }

        #endregion Public properties

        #region Bindable collections

        public BindableCollection<Doctor> Doctors
        {
            get { return _doctors; }
            set { _doctors = value; NotifyOfPropertyChange(() => Doctors); }
        }

        public BindableCollection<Pacient> Pacients
        {
            get { return pacients; }
            set { pacients = value; NotifyOfPropertyChange(() => Pacients); }
        }

        public BindableCollection<PacientTypeView> PacientTypes
        {
            get { return _pacientTypes; }
            set { _pacientTypes = value; NotifyOfPropertyChange(() => PacientTypes); }
        }

        #endregion Bindable collections

        #region Methods

        public void ClearFilter()
        {
            FirstNameForFilter = null;
            LastNameForFilter = null;
            PatronymicNameForFilter = null;
            PacientPhoneNumberForFilter = null;
            ParentPhoneNumberForFilter = null;
            PacientTypeForFilter = null;
        }

        public async void DelToken()
        {
            await Model.DeleteToken();
        }

        public void FilterPacients()
        {
            IsPacientsLoading = true;

            var pac = Model.PacientsList.Select(p => p).ToList();

            if (pac != null)
            {
                if (!string.IsNullOrEmpty(LastNameForFilter))
                {
                    pac = pac.Where(p => p.LastName.ToLower().Contains(LastNameForFilter.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(FirstNameForFilter))
                {
                    pac = pac.Where(p => p.FirstName.ToLower().Contains(FirstNameForFilter.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(PatronymicNameForFilter))
                {
                    pac = pac.Where(p => p.PatronymicName.ToLower().Contains(PatronymicNameForFilter.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(PacientPhoneNumberForFilter))
                {
                    pac = pac.Where(p => p.PacientPhoneNumber.Contains(PacientPhoneNumberForFilter)).ToList();
                }

                if (!string.IsNullOrEmpty(ParentPhoneNumberForFilter))
                {
                    pac = pac.Where(p => p.ParentsPhoneNumber.Contains(ParentPhoneNumberForFilter)).ToList();
                }

                if (PacientTypeForFilter != null)
                {
                    pac = pac.Where(p => p.PacientType == PacientTypeForFilter.Value).ToList();
                }

                Pacients.Clear();
                Pacients.AddRange(pac);
            }
            IsPacientsLoading = false;
        }

        public async void SetVisit()
        {
            if (SelectedPacient != null && SelectedDoctor != null)
            {
                var ok = await Model.SetPacientVisitAsync(SelectedPacient, SelectedDoctor);

                if (ok)
                {
                    ChooseDoctor = false;

                    Timer.Start();
                }
                else
                    MessageBox.Show("Ошибка соединения с сервером, повторите действие");
            }
        }

        public void ShowArrivalDialog()
        {
            Timer.Stop();

            SelectedDoctor = null;
            ChooseDoctor = true;
        }

        public async void ShowPacientDocumet()
        {
            if (SelectedPacient != null)
            {
                await Model.OpenPacientDocument(SelectedPacient);
            }
        }

        public void ShowVisitLogs()
        {
            if (SelectedPacient != null)
            {
                var visitLogViewModel = new VisitLogViewModel(SelectedPacient, Model.WebClient);

                WindowManager.ShowDialog(visitLogViewModel);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            FilterPacients();

            NotifyOfPropertyChange(() => CanSetVisit);
        }

        #endregion Methods

        private PacientsListModel Model;
        private IWindowManager WindowManager;

        public PacientsListViewModel(IWindowManager theWindowManager, PacientsListModel model)
        {
            Model = model;

            Timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(15)
            };
            Timer.Tick += Timer_Tick;
            Timer.Start();

            WindowManager = theWindowManager;
        }

        public bool CanClearFilter
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstNameForFilter) || !string.IsNullOrEmpty(LastNameForFilter) || !string.IsNullOrEmpty(PatronymicNameForFilter)
                    || !string.IsNullOrEmpty(PacientPhoneNumberForFilter) || !string.IsNullOrEmpty(ParentPhoneNumberForFilter)
                    || PacientTypeForFilter != null)
                    return true;
                else return false;
            }
        }

        public bool CanSetVisit
        {
            get
            {
                if (SelectedPacient != null && SelectedDoctor != null)
                    return true;
                else return false;
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override async void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            Model.ClearTempFolder();

            await Model.DeleteToken();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Task.Run(async () =>
            {
                try
                {
                    IsPacientsLoading = true;

                    await Model.GetProgrammTokenAsync();

                    var doc = await Model.GetDoctorsAsync();

                    if (doc != null)
                        Doctors.AddRange(doc);

                    await Model.GetPacientsAsync();

                    var values = Enum.GetValues(typeof(EPatientType)).Cast<EPatientType>().ToList();

                    values.ForEach(v =>
                    {
                        PacientTypes.Add(new PacientTypeView { Name = v.GetDescription(), Value = v });
                    });

                    FilterPacients();

                    IsPacientsLoading = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK);
                }
            });
        }

        public class PacientTypeView
        {
            public string Name { get; set; }
            public EPatientType Value { get; set; }
        }

        public void CancelSetVisit()
        {
            Timer.Start();
            ChooseDoctor = false;
        }
    }
}