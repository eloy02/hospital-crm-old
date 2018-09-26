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

        private Pacient _pacient;
        private BindableCollection<BuildingsView> _buildings = new BindableCollection<BuildingsView>();
        private string _pdfFilePath;
        private EPatientType? _selectedPacientType;
        private PacientInfoModel Model;

        public PacientInfoViewModel(Pacient pacient, Guid webToken)
        {
            this.Pacient = pacient;
            this.WebToken = webToken;
            Model = new PacientInfoModel(this, WebToken);

            SelectedPacientType = Pacient.PacientType;
        }

        public Pacient Pacient
        {
            get { return _pacient; }
            set { _pacient = value; NotifyOfPropertyChange(() => Pacient); }
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
                    Pacient.PacientType = SelectedPacientType.Value;
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
            await Model.OpenPacientDocument(Pacient);
        }

        public async void UpdatePacient()
        {
            var pacient = new Pacient();
            pacient = Pacient;

            if (!string.IsNullOrEmpty(PdfPath))
            {
                Pacient.DocumentPath = PdfPath;
            }

            await Model.UpdatePacientAsync(Pacient);
        }
    }
}