using Caliburn.Micro;
using Core.Types;
using Core.Types.Enumerations;
using MaterialDesignThemes.Wpf;
using RehabilitationCentre.Models;
using System;
using System.Linq;

namespace RehabilitationCentre.ViewModels
{
    public class PacientsListViewModel : Screen
    {
        public class PacientTypeView
        {
            public string Name { get; set; }
            public EPatientType Value { get; set; }
        }

        private BindableCollection<Pacient> pacients = new BindableCollection<Pacient>();
        private bool _chooseDoctor = false;
        private PacientsListModel Model;
        private bool _isPacientsLoading = false;
        private Pacient _selectedPacient;
        private BindableCollection<Doctor> _doctors = new BindableCollection<Doctor>();
        private Doctor _selectedDoctor;
        private string _lastNameForFilter;
        private string _firstNameForFilter;
        private string _patronymicNameForFilter;
        private string _pacientPhoneNumberForFilter;
        private string _parentPhoneNumberForFilter;
        private BindableCollection<PacientTypeView> _pacientTypes = new BindableCollection<PacientTypeView>();
        private PacientTypeView _pacientTypeForFilter;

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

        public BindableCollection<PacientTypeView> PacientTypes
        {
            get { return _pacientTypes; }
            set { _pacientTypes = value; NotifyOfPropertyChange(() => PacientTypes); }
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

        public BindableCollection<Doctor> Doctors
        {
            get { return _doctors; }
            set { _doctors = value; NotifyOfPropertyChange(() => Doctors); }
        }

        public Pacient SelectedPacient
        {
            get { return _selectedPacient; }
            set { _selectedPacient = value; NotifyOfPropertyChange(() => SelectedPacient); }
        }

        public bool IsPacientsLoading
        {
            get { return _isPacientsLoading; }
            set { _isPacientsLoading = value; NotifyOfPropertyChange(() => IsPacientsLoading); }
        }

        public PacientsListViewModel()
        {
        }

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

        public BindableCollection<Pacient> Pacients
        {
            get { return pacients; }
            set { pacients = value; NotifyOfPropertyChange(() => Pacients); }
        }

        protected override async void OnInitialize()
        {
            Model = new PacientsListModel(this);

            await Model.GetDoctorsAsync();

            var values = Enum.GetValues(typeof(EPatientType)).Cast<EPatientType>().ToList();
            values.ForEach(v =>
            {
                PacientTypes.Add(new PacientTypeView { Name = v.GetDescription(), Value = v });
            });

            base.OnInitialize();
        }

        protected override async void OnActivate()
        {
            IsPacientsLoading = true;

            var r = await Model.GetPacientsAsync();

            if (r != null)
            {
                Pacients.Clear();
                Pacients.AddRange(r);
            }

            IsPacientsLoading = false;

            base.OnActivate();
        }

        public async void ShowPacientDocumet()
        {
            if (SelectedPacient != null)
            {
                await Model.OpenPacientDocument(SelectedPacient);
            }
        }

        public void ShowArrivalDialog()
        {
            ChooseDoctor = true;
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

        public async void SetVisit()
        {
            if (SelectedPacient != null && SelectedDoctor != null)
            {
                await Model.SetPacientVisitAsync(SelectedPacient, SelectedDoctor);
            }
        }

        public async void FilterPacients()
        {
            IsPacientsLoading = true;
            var pac = await Model.GetPacientsAsync();

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

        public void ClearFilter()
        {
            FirstNameForFilter = null;
            LastNameForFilter = null;
            PatronymicNameForFilter = null;
            PacientPhoneNumberForFilter = null;
            ParentPhoneNumberForFilter = null;
            PacientTypeForFilter = null;
        }
    }
}