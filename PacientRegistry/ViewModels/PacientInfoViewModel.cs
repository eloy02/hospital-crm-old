using Caliburn.Micro;
using Core.Types;
using Core.Types.Enumerations;
using PacientRegistry.Models;
using System;
using static PacientRegistry.ShellViewModel;

namespace PacientRegistry.ViewModels
{
    public class PacientInfoViewModel : Conductor<object>
    {
        private Guid WebToken;

        private Pacient _pacientOld;
        private BindableCollection<BuildingsView> _buildings = new BindableCollection<BuildingsView>();
        private string _pdfFilePath;
        private EPatientType? _selectedPacientType;
        private PacientInfoModel Model;
        private Pacient _pacient;

        public Pacient Pacient
        {
            get { return _pacient; }
            set { _pacient = value; NotifyOfPropertyChange(() => Pacient); }
        }

        public PacientInfoViewModel(Pacient pacient, Guid webToken)
        {
            this.PacientOld = pacient;
            this.WebToken = webToken;
            Model = new PacientInfoModel(this, WebToken);

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

        public static BindableCollection<PacientTypeView> PacientTypes
        {
            get { return _pacientTypes; }
        }

        private static BindableCollection<PacientTypeView> _pacientTypes = new BindableCollection<PacientTypeView>()
        {
            new PacientTypeView{ Type = EPatientType.Invalid, Name = "Инвалид"},
            new PacientTypeView{ Type = EPatientType.OVZ, Name = "ОВЗ"}
        };

        public async void ShowPDFDocument()
        {
            await Model.OpenPacientDocument(PacientOld);
        }

        public async void UpdatePacient()
        {
            PacientOld = Pacient;

            if (!string.IsNullOrEmpty(PdfPath))
            {
                PacientOld.DocumentPath = PdfPath;
            }

            await Model.UpdatePacientAsync(PacientOld);
        }
    }
}