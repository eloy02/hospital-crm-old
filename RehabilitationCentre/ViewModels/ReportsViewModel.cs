using Caliburn.Micro;
using Core.Types;
using MaterialDesignThemes.Wpf;
using RehabilitationCentre.Models;

namespace RehabilitationCentre.ViewModels
{
    public class ReportsViewModel : Screen
    {
        public static object Icon
        {
            get { return PackIconKind.Notebook; }
        }

        public static string Name
        {
            get { return "Отчеты"; }
        }

        private ReportsModel Model;
        private BindableCollection<Pacient> pacients = new BindableCollection<Pacient>();
        private BindableCollection<Doctor> _doctors = new BindableCollection<Doctor>();
        private Doctor _selectedDoctor;
        private Pacient _selectedPacient;
        private string _doctorFioForSearch;
        private string _pacientFioForSearch;

        private bool _isWaitingReport = false;

        public bool IsWaitingReport
        {
            get { return _isWaitingReport; }
            set
            {
                _isWaitingReport = value; NotifyOfPropertyChange(() => IsWaitingReport);
                NotifyOfPropertyChange(() => CanCreateDoctorsReport);
                NotifyOfPropertyChange(() => CanCreateReport);
            }
        }

        public BindableCollection<Pacient> Pacients
        {
            get { return pacients; }
            set { pacients = value; NotifyOfPropertyChange(() => Pacients); }
        }

        public BindableCollection<Doctor> Doctors
        {
            get { return _doctors; }
            set { _doctors = value; NotifyOfPropertyChange(() => Doctors); }
        }

        public Doctor SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value; NotifyOfPropertyChange(() => SelectedDoctor);
                NotifyOfPropertyChange(() => CanCreateDoctorsReport);
            }
        }

        public Pacient SelectedPacient
        {
            get { return _selectedPacient; }
            set
            {
                _selectedPacient = value; NotifyOfPropertyChange(() => SelectedPacient);
                NotifyOfPropertyChange(() => CanCreateReport);
            }
        }

        public string PacientFioForSearch
        {
            get { return _pacientFioForSearch; }
            set { _pacientFioForSearch = value; NotifyOfPropertyChange(() => PacientFioForSearch); }
        }

        public string DoctorFioForSearch
        {
            get { return _doctorFioForSearch; }
            set { _doctorFioForSearch = value; NotifyOfPropertyChange(() => DoctorFioForSearch); }
        }

        public ReportsViewModel(ReportsModel model)
        {
            this.Model = model;
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();

            var pacients = await Model.GetPacientsAsync();
            if (pacients != null)
            {
                Pacients.Clear();
                Pacients.AddRange(pacients);
            }

            var doctors = await Model.GetDoctorsAsync();
            if (doctors != null)
            {
                Doctors.Clear();
                Doctors.AddRange(doctors);
            }
        }

        public async void CreateReport()
        {
            if (SelectedPacient != null)
            {
                IsWaitingReport = true;

                await Model.CreateVisitReportAsync(SelectedPacient);

                IsWaitingReport = false;
            }
        }

        public bool CanCreateReport
        {
            get
            {
                if (SelectedPacient != null && IsWaitingReport == false)
                    return true;
                else return false;
            }
        }

        public bool CanCreateDoctorsReport
        {
            get
            {
                if (SelectedDoctor != null && IsWaitingReport == false)
                    return true;
                else return false;
            }
        }

        public async void CreateDoctorsReport()
        {
            if (SelectedDoctor != null)
            {
                IsWaitingReport = true;

                await Model.CreateVisitReportAsync(SelectedDoctor);

                IsWaitingReport = false;
            }
        }
    }
}