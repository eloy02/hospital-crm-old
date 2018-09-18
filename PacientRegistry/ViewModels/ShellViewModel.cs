using Caliburn.Micro;
using Core.Types;
using Core.Types.Enumerations;
using KladrApiClient;
using PacientRegistry.Models;
using System;
using System.Linq;
using System.Windows;

namespace PacientRegistry
{
    public class ShellViewModel : Conductor<object>, IShell
    {
        private static BindableCollection<PacientTypeView> _pacientTypes = new BindableCollection<PacientTypeView>()
        {
            new PacientTypeView{ Type = EPatientType.Invalid, Name = "Инвалид"},
            new PacientTypeView{ Type = EPatientType.OVZ, Name = "ОВЗ"}
        };

        private string _buildingNum;
        private BindableCollection<BuildingsView> _buildings = new BindableCollection<BuildingsView>();
        private Visibility _buildingsLoadingVisibility = Visibility.Collapsed;
        private string _flatNum;
        private Visibility _flatsLoadingVisibility = Visibility.Collapsed;
        private string _pacientFirstName;
        private string _pacientLastName;
        private string _pacientPatronymicName;
        private string _pacientPhoneNumber;
        private string _parentFirstName;
        private string _parentLastName;
        private string _parentPatronymicName;
        private string _parentPhoneNumver;
        private string _pdfFilePath;
        private Visibility _savingPacientVisibility = Visibility.Collapsed;
        private BuildingsView _selectedBuilding;
        private EPatientType? _selectedPacientType;
        private SitiesView _selectedSity;
        private StreetsView _selectedStreet;
        private BindableCollection<SitiesView> _sities = new BindableCollection<SitiesView>();
        private Visibility _sitiesLoadingVisibility = Visibility.Collapsed;
        private string _sity;
        private string _street;
        private BindableCollection<StreetsView> _streets = new BindableCollection<StreetsView>();
        private Visibility _streetsLoadingVisibility = Visibility.Collapsed;
        private ShellModel Model;
        private Pacient _selectedPacient;
        private BindableCollection<Pacient> pacients = new BindableCollection<Pacient>();

        #region Visibility

        public Visibility BuildingsLoadingVisibility
        {
            get { return _buildingsLoadingVisibility; }
            set { _buildingsLoadingVisibility = value; NotifyOfPropertyChange(() => BuildingsLoadingVisibility); }
        }

        public Visibility FlatsLoadingVisibility
        {
            get { return _flatsLoadingVisibility; }
            set { _flatsLoadingVisibility = value; NotifyOfPropertyChange(() => FlatsLoadingVisibility); }
        }

        public Visibility SavingPacientVisibility
        {
            get { return _savingPacientVisibility; }
            set { _savingPacientVisibility = value; NotifyOfPropertyChange(() => SavingPacientVisibility); }
        }

        public Visibility SitiesLoadingVisibility
        {
            get { return _sitiesLoadingVisibility; }
            set { _sitiesLoadingVisibility = value; NotifyOfPropertyChange(() => SitiesLoadingVisibility); }
        }

        public Visibility StreetsLoadingVisibility
        {
            get { return _streetsLoadingVisibility; }
            set { _streetsLoadingVisibility = value; NotifyOfPropertyChange(() => StreetsLoadingVisibility); }
        }

        #endregion Visibility

        public ShellViewModel()
        {
            Model = new ShellModel(this);
            Model.LoadSities();
        }

        protected override async void OnActivate()
        {
            var r = await Model.GetPacientsAsync();

            if (r != null)
            {
                Pacients.Clear();
                Pacients.AddRange(r);
            }

            base.OnActivate();
        }

        public Pacient SelectedPacient
        {
            get { return _selectedPacient; }
            set { _selectedPacient = value; NotifyOfPropertyChange(() => SelectedPacient); }
        }

        public BindableCollection<Pacient> Pacients
        {
            get { return pacients; }
            set { pacients = value; NotifyOfPropertyChange(() => Pacients); }
        }

        public static BindableCollection<PacientTypeView> PacientTypes
        {
            get { return _pacientTypes; }
        }

        public string BuildingNumber
        {
            get { return _buildingNum; }
            set
            {
                _buildingNum = value;
                NotifyOfPropertyChange(() => BuildingNumber);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public BindableCollection<BuildingsView> Buildings
        {
            get { return _buildings; }
            set { _buildings = value; NotifyOfPropertyChange(() => Buildings); }
        }

        public string FlatNumber
        {
            get { return _flatNum; }
            set
            {
                _flatNum = value;
                NotifyOfPropertyChange(() => FlatNumber);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public string PacientFirstName
        {
            get { return _pacientFirstName; }
            set
            {
                _pacientFirstName = value;
                NotifyOfPropertyChange(() => PacientFirstName);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public string PacientLastName
        {
            get { return _pacientLastName; }
            set
            {
                _pacientLastName = value;
                NotifyOfPropertyChange(() => PacientLastName);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public string PacientPatronymicName
        {
            get { return _pacientPatronymicName; }
            set
            {
                _pacientPatronymicName = value;
                NotifyOfPropertyChange(() => PacientPatronymicName);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public string PacientPhoneNumber
        {
            get { return _pacientPhoneNumber; }
            set
            {
                _pacientPhoneNumber = value;
                NotifyOfPropertyChange(() => PacientPhoneNumber);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public string ParentFirstName
        {
            get { return _parentFirstName; }
            set
            {
                _parentFirstName = value;
                NotifyOfPropertyChange(() => ParentFirstName);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public string ParentLastName
        {
            get { return _parentLastName; }
            set
            {
                _parentLastName = value;
                NotifyOfPropertyChange(() => ParentLastName);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public string ParentPatronymicName
        {
            get { return _parentPatronymicName; }
            set
            {
                _parentPatronymicName = value;
                NotifyOfPropertyChange(() => ParentPatronymicName);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public string ParentPhoneNumber
        {
            get { return _parentPhoneNumver; }
            set
            {
                _parentPhoneNumver = value;
                NotifyOfPropertyChange(() => ParentPhoneNumber);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public string PdfPath
        {
            get { return _pdfFilePath; }
            set
            {
                _pdfFilePath = value;
                NotifyOfPropertyChange(() => PdfPath);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public BuildingsView SelectedBuilding
        {
            get { return _selectedBuilding; }
            set
            {
                _selectedBuilding = value;
                FlatNumber = string.Empty;
                NotifyOfPropertyChange(() => SelectedBuilding);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public EPatientType? SelectedPacientType
        {
            get { return _selectedPacientType; }
            set
            {
                _selectedPacientType = value ?? null;
                NotifyOfPropertyChange(() => SelectedPacientType);
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public SitiesView SelectedSity
        {
            get { return _selectedSity; }
            set
            {
                _selectedSity = value;
                NotifyOfPropertyChange(() => SelectedSity);
                Buildings.Clear();
                Streets.Clear();
                FlatNumber = null;

                if (SelectedSity != null)
                {
                    StreetsLoadingVisibility = Visibility.Visible;
                    Model.LoadStreetsForSity(SelectedSity.Code);
                }
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public StreetsView SelectedStreet
        {
            get { return _selectedStreet; }
            set
            {
                _selectedStreet = value;
                NotifyOfPropertyChange(() => SelectedStreet);
                Buildings.Clear();
                FlatNumber = null;
                if (SelectedStreet != null)
                {
                    BuildingsLoadingVisibility = Visibility.Visible;
                    Model.LoadBuildingsForStreet(SelectedStreet.Code);
                }
                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public BindableCollection<SitiesView> Sities
        {
            get { return _sities; }
            set
            {
                _sities = value;
                NotifyOfPropertyChange(() => Sities);
            }
        }

        public string Sity
        {
            get { return _sity; }
            set
            {
                _sity = value;
                NotifyOfPropertyChange(() => Sity);
                if (SelectedSity == null)
                {
                    SitiesLoadingVisibility = Visibility.Visible;
                    Model.SearchSity(Sity);
                }

                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                NotifyOfPropertyChange(() => Street);
                if (SelectedStreet == null && SelectedSity != null)
                {
                    StreetsLoadingVisibility = Visibility.Visible;
                    Model.SearchStreets(SelectedSity.Code, Street);
                }

                NotifyOfPropertyChange(() => CanClearForms);
                NotifyOfPropertyChange(() => CanSavePacient);
            }
        }

        public BindableCollection<StreetsView> Streets
        {
            get { return _streets; }
            set { _streets = value; NotifyOfPropertyChange(() => Streets); }
        }

        public bool CanClearForms
        {
            get
            {
                if (
                    SelectedBuilding != null || SelectedPacientType != null || !string.IsNullOrEmpty(FlatNumber)
                    || SelectedSity != null || SelectedStreet != null
                    || !string.IsNullOrEmpty(Street) || !string.IsNullOrEmpty(BuildingNumber)
                    || !string.IsNullOrEmpty(Sity) || !string.IsNullOrEmpty(PacientFirstName)
                    || !string.IsNullOrEmpty(PacientLastName) || !string.IsNullOrEmpty(PacientPatronymicName)
                    || !string.IsNullOrEmpty(PacientPhoneNumber) || !string.IsNullOrEmpty(ParentFirstName)
                    || !string.IsNullOrEmpty(ParentLastName) || !string.IsNullOrEmpty(ParentPhoneNumber)
                    || !string.IsNullOrEmpty(ParentPatronymicName) || !string.IsNullOrEmpty(PdfPath))
                    return true;
                else return false;
            }
        }

        public void ClearForms()
        {
            SelectedBuilding = null;
            SelectedPacientType = null;
            SelectedSity = null;
            SelectedStreet = null;
            Street = null;
            BuildingNumber = null;
            Sity = null;
            PacientFirstName = null;
            PacientLastName = null;
            PacientPatronymicName = null;
            PacientPhoneNumber = null;
            ParentFirstName = null;
            ParentLastName = null;
            ParentPhoneNumber = null;
            ParentPatronymicName = null;
            PdfPath = null;
        }

        public bool CanSavePacient
        {
            get
            {
                if (
                    !string.IsNullOrEmpty(Street) && !string.IsNullOrEmpty(BuildingNumber)
                    && !string.IsNullOrEmpty(Sity) && !string.IsNullOrEmpty(PacientFirstName)
                    && !string.IsNullOrEmpty(PacientLastName)
                    && !string.IsNullOrEmpty(PdfPath)
                    && (!string.IsNullOrEmpty(ParentPhoneNumber) || !string.IsNullOrEmpty(PacientPhoneNumber)))
                    return true;
                else return false;
            }
            set { }
        }

        public async void SavePacient()
        {
            SavingPacientVisibility = Visibility.Visible;

            try
            {
                await Model.SavePacientAsync();

                ClearForms();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Регистратура", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            SavingPacientVisibility = Visibility.Collapsed;
        }

        public void SetKladrBuildings(KladrResponse response)
        {
            var rr = response.result.ToList();

            if (rr.Count > 1)
            {
                Buildings.Clear();
                rr.ForEach(r => { var s = new BuildingsView { Name = $"{r.typeShort}. {r.name}", Code = r.id }; Buildings.Add(s); });
            }
            BuildingsLoadingVisibility = Visibility.Collapsed;
        }

        public void SetKladrSities(KladrResponse response)
        {
            var rr = response.result.ToList();

            if (rr.Count > 1)
            {
                Sities.Clear();
                rr.ForEach(r => { var s = new SitiesView { Name = $"{r.typeShort}. {r.name}", Code = r.id }; Sities.Add(s); });
            }
            SitiesLoadingVisibility = Visibility.Collapsed;
        }

        public void SetKladrStreets(KladrResponse response)
        {
            var rr = response.result.ToList();

            if (rr.Count > 1)
            {
                Streets.Clear();
                rr.ForEach(r => { var s = new StreetsView { Name = $"{r.typeShort}. {r.name}", Code = r.id }; Streets.Add(s); });
            }
            StreetsLoadingVisibility = Visibility.Collapsed;
        }

        public class BuildingsView
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public class PacientTypeView
        {
            public string Name { get; set; }
            public EPatientType Type { get; set; }
        }

        public class SitiesView
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public class StreetsView
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }
}