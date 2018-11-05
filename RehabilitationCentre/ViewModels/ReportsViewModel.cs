using Caliburn.Micro;
using Core.Types;
using MaterialDesignThemes.Wpf;
using RehabilitationCentre.Models;
using System;
using System.Linq;

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
        private DateTime? _visitDateTo;
        private DateTime? _visitDateFrom;
        private DateTime? _doctorVisitDateTo;
        private DateTime? _doctorVisitDateFrom;

        public DateTime? DoctorVisitDateFrom
        {
            get { return _doctorVisitDateFrom; }
            set
            {
                _doctorVisitDateFrom = value;
                NotifyOfPropertyChange(() => DoctorVisitDateFrom);
                NotifyOfPropertyChange(() => CanClearDoctorVisitDateFrom);
            }
        }

        public DateTime? DoctorVisitDateTo
        {
            get { return _doctorVisitDateTo; }
            set
            {
                _doctorVisitDateTo = value;
                NotifyOfPropertyChange(() => DoctorVisitDateTo);
                NotifyOfPropertyChange(() => CanClearDoctorVisitDateTo);
            }
        }

        public DateTime? VisitDateFrom
        {
            get { return _visitDateFrom; }
            set
            {
                _visitDateFrom = value; NotifyOfPropertyChange(() => VisitDateFrom);
                NotifyOfPropertyChange(() => CanClearVisitDateFrom);
            }
        }

        public DateTime? VisitDateTo
        {
            get { return _visitDateTo; }
            set
            {
                _visitDateTo = value; NotifyOfPropertyChange(() => VisitDateTo);
                NotifyOfPropertyChange(() => CanClearVisitDateTo);
            }
        }

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
            set { _pacientFioForSearch = value; NotifyOfPropertyChange(() => PacientFioForSearch); SearchPacient(); }
        }

        public string DoctorFioForSearch
        {
            get { return _doctorFioForSearch; }
            set
            {
                _doctorFioForSearch = value; NotifyOfPropertyChange(() => DoctorFioForSearch);
                SearchDoctor();
            }
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

                await Model.CreateVisitReportAsync(SelectedPacient, VisitDateFrom, VisitDateTo);

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

                await Model.CreateVisitReportAsync(SelectedDoctor, DoctorVisitDateFrom, DoctorVisitDateTo);

                IsWaitingReport = false;
            }
        }

        public void SearchDoctor()
        {
            var doctors = Model.Doctors.Select(d => d).ToList();

            var r = doctors.Where(d => !string.IsNullOrEmpty(d.FIO)).Where(d => d.FIO.Contains(DoctorFioForSearch)).ToList();

            Doctors.Clear();
            Doctors.AddRange(r);
            Doctors.Refresh();
        }

        public void SearchPacient()
        {
            var pacients = Model.PacientsList.Select(p => p).ToList();

            var r = pacients.Where(p => !string.IsNullOrEmpty(p.FIO)).Where(p => p.FIO.Contains(PacientFioForSearch)).ToList();

            Pacients.Clear();
            Pacients.AddRange(r);
            Pacients.Refresh();
        }

        public void ClearVisitDateTo()
        {
            VisitDateTo = null;
        }

        public void ClearVisitDateFrom()
        {
            VisitDateFrom = null;
        }

        public bool CanClearVisitDateTo
        {
            get
            {
                if (VisitDateTo != null)
                    return true;
                else return false;
            }
        }

        public bool CanClearVisitDateFrom
        {
            get
            {
                return !(VisitDateFrom is null);
            }
        }

        public void ClearDoctorVisitDateFrom()
        {
            DoctorVisitDateFrom = null;
        }

        public void ClearDoctorVisitDateTo()
        {
            DoctorVisitDateTo = null;
        }

        public bool CanClearDoctorVisitDateTo
        {
            get
            {
                return !(DoctorVisitDateTo is null);
            }
        }

        public bool CanClearDoctorVisitDateFrom
        {
            get
            {
                return !(DoctorVisitDateFrom is null);
            }
        }
    }
}