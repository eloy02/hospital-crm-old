using Caliburn.Micro;
using Core.Types;
using Core.Types.Enumerations;
using PacientRegistry.Models;
using System.Windows;
using WebClient.Interfaces;
using static PacientRegistry.ShellViewModel;

namespace PacientRegistry.ViewModels
{
    public class PacientInfoViewModel : Screen
    {
        private Pacient _pacientOld;
        private BindableCollection<BuildingsView> _buildings = new BindableCollection<BuildingsView>();
        private string _pdfFilePath;
        private EPatientType? _selectedPacientType;
        private PacientInfoModel Model;
        private Pacient _pacient;

        private bool _updatingPacientButtonVisibility = true;

        public bool UpdatingPacientButtonVisibility
        {
            get { return _updatingPacientButtonVisibility; }
            set { _updatingPacientButtonVisibility = value; NotifyOfPropertyChange(() => UpdatingPacientButtonVisibility); }
        }

        private Visibility _updatingPacientVisibility = Visibility.Collapsed;

        public Visibility UpdatingPacientVisibility
        {
            get { return _updatingPacientVisibility; }
            set { _updatingPacientVisibility = value; NotifyOfPropertyChange(() => UpdatingPacientVisibility); }
        }

        private bool _documentShowEnablement = true;

        public bool DocumentShowEnablement
        {
            get { return _documentShowEnablement; }
            set { _documentShowEnablement = value; NotifyOfPropertyChange(() => DocumentShowEnablement); }
        }

        private Visibility _documentLoadingVisibility = Visibility.Hidden;

        public Visibility DocumentLoadingVisibility
        {
            get
            {
                return _documentLoadingVisibility;
            }
            set
            {
                _documentLoadingVisibility = value; NotifyOfPropertyChange(() => DocumentLoadingVisibility);
            }
        }

        public Pacient Pacient
        {
            get { return _pacient; }
            set { _pacient = value; NotifyOfPropertyChange(() => Pacient); }
        }

        public PacientInfoViewModel(Pacient pacient, IWebClient webClient)
        {
            this.PacientOld = pacient;
            Model = new PacientInfoModel(webClient);

            Pacient = new Pacient()
            {
                BuildingNumber = pacient.BuildingNumber,
                FirstName = pacient.FirstName,
                FlatNumber = pacient.FlatNumber,
                Id = pacient.Id,
                LastName = pacient.LastName,
                PacientPhoneNumber = pacient.PacientPhoneNumber,
                PacientType = pacient.PacientType,
                ParentFirstName = pacient.ParentFirstName,
                ParentLastName = pacient.ParentLastName,
                ParentPatronymicName = pacient.ParentPatronymicName,
                ParentsPhoneNumber = pacient.ParentsPhoneNumber,
                Sity = pacient.Sity,
                PatronymicName = pacient.PatronymicName,
                Street = pacient.Street
            };

            SelectedPacientType = Pacient.PacientType;
        }

        public Pacient PacientOld
        {
            get { return _pacientOld; }
            set { _pacientOld = value; NotifyOfPropertyChange(() => PacientOld); }
        }

        public string PdfPath
        {
            get { return _pdfFilePath; }
            set
            {
                _pdfFilePath = value;
                NotifyOfPropertyChange(() => PdfPath);
            }
        }

        public EPatientType? SelectedPacientType
        {
            get { return _selectedPacientType; }
            set
            {
                _selectedPacientType = value ?? null;

                if (SelectedPacientType != null)
                    PacientOld.PacientType = SelectedPacientType.Value;
                NotifyOfPropertyChange(() => SelectedPacientType);
            }
        }

        public static BindableCollection<PacientTypeView> PacientTypes { get; } = new BindableCollection<PacientTypeView>()
        {
            new PacientTypeView{ Type = EPatientType.Invalid, Name = "Инвалид"},
            new PacientTypeView{ Type = EPatientType.OVZ, Name = "ОВЗ"}
        };

        public async void ShowPDFDocument()
        {
            DocumentLoadingVisibility = Visibility.Visible;
            DocumentShowEnablement = false;

            await Model.OpenPacientDocument(PacientOld);

            DocumentLoadingVisibility = Visibility.Hidden;
            DocumentShowEnablement = true;
        }

        public async void UpdatePacient()
        {
            UpdatingPacientButtonVisibility = false;
            UpdatingPacientVisibility = Visibility.Visible;
            PacientOld = Pacient;

            if (!string.IsNullOrEmpty(PdfPath))
            {
                PacientOld.DocumentPath = PdfPath;
            }

            var ok = await Model.UpdatePacientAsync(PacientOld);

            if (ok == false)
                MessageBox.Show("Ошибка сервера. Повторите действие еще раз.");

            UpdatingPacientButtonVisibility = true;
            UpdatingPacientVisibility = Visibility.Collapsed;

            if (ok)
                TryClose();
        }
    }
}