using Caliburn.Micro;
using KladrApiClient;
using PacientRegistry.Models;
using System.Linq;
using System.Windows;
using —ore.Types.Enumerations;

namespace PacientRegistry
{
    public class ShellViewModel : PropertyChangedBase, IShell
    {
        private static BindableCollection<PacientTypeView> _pacientTypes = new BindableCollection<PacientTypeView>()
        {
            new PacientTypeView{ Type = EPatientType.Invalid, Name = "»Ì‚‡ÎË‰"},
            new PacientTypeView{ Type = EPatientType.OVZ, Name = "Œ¬«"}
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

        public ShellViewModel()
        {
            Model = new ShellModel(this);
            Model.LoadSities();
        }

        public static BindableCollection<PacientTypeView> PacientTypes
        {
            get { return _pacientTypes; }
        }

        public string BuildingNumber
        {
            get { return _buildingNum; }
            set { _buildingNum = value; NotifyOfPropertyChange(() => BuildingNumber); }
        }

        public BindableCollection<BuildingsView> Buildings
        {
            get { return _buildings; }
            set { _buildings = value; NotifyOfPropertyChange(() => Buildings); }
        }

        public Visibility BuildingsLoadingVisibility
        {
            get { return _buildingsLoadingVisibility; }
            set { _buildingsLoadingVisibility = value; NotifyOfPropertyChange(() => BuildingsLoadingVisibility); }
        }

        public string FlatNumber
        {
            get { return _flatNum; }
            set { _flatNum = value; NotifyOfPropertyChange(() => FlatNumber); }
        }

        public Visibility FlatsLoadingVisibility
        {
            get { return _flatsLoadingVisibility; }
            set { _flatsLoadingVisibility = value; NotifyOfPropertyChange(() => FlatsLoadingVisibility); }
        }

        public string PacientFirstName
        {
            get { return _pacientFirstName; }
            set { _pacientFirstName = value; NotifyOfPropertyChange(() => PacientFirstName); }
        }

        public string PacientLastName
        {
            get { return _pacientLastName; }
            set { _pacientLastName = value; NotifyOfPropertyChange(() => PacientLastName); }
        }

        public string PacientPatronymicName
        {
            get { return _pacientPatronymicName; }
            set { _pacientPatronymicName = value; NotifyOfPropertyChange(() => PacientPatronymicName); }
        }

        public string PacientPhoneNumber
        {
            get { return _pacientPhoneNumber; }
            set { _pacientPhoneNumber = value; NotifyOfPropertyChange(() => PacientPhoneNumber); }
        }

        public Visibility SavingPacientVisibility
        {
            get { return _savingPacientVisibility; }
            set { _savingPacientVisibility = value; NotifyOfPropertyChange(() => SavingPacientVisibility); }
        }

        public BuildingsView SelectedBuilding
        {
            get { return _selectedBuilding; }
            set { _selectedBuilding = value; NotifyOfPropertyChange(() => SelectedBuilding); }
        }

        public EPatientType? SelectedPacientType
        {
            get { return _selectedPacientType; }
            set { _selectedPacientType = value ?? null; NotifyOfPropertyChange(() => SelectedPacientType); }
        }

        public SitiesView SelectedSity
        {
            get { return _selectedSity; }
            set
            {
                _selectedSity = value;
                NotifyOfPropertyChange(() => SelectedSity);

                if (SelectedSity != null)
                {
                    StreetsLoadingVisibility = Visibility.Visible;
                    Model.LoadStreetsForSity(SelectedSity.Code);
                }
            }
        }

        public StreetsView SelectedStreet
        {
            get { return _selectedStreet; }
            set
            {
                _selectedStreet = value;
                NotifyOfPropertyChange(() => SelectedStreet);
                if (SelectedStreet != null)
                {
                    BuildingsLoadingVisibility = Visibility.Visible;
                    Model.LoadBuildingsForStreet(SelectedStreet.Code);
                }
            }
        }

        public BindableCollection<SitiesView> Sities
        {
            get { return _sities; }
            set { _sities = value; NotifyOfPropertyChange(() => Sities); }
        }

        public Visibility SitiesLoadingVisibility
        {
            get { return _sitiesLoadingVisibility; }
            set { _sitiesLoadingVisibility = value; NotifyOfPropertyChange(() => SitiesLoadingVisibility); }
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
            }
        }

        public BindableCollection<StreetsView> Streets
        {
            get { return _streets; }
            set { _streets = value; NotifyOfPropertyChange(() => Streets); }
        }

        public Visibility StreetsLoadingVisibility
        {
            get { return _streetsLoadingVisibility; }
            set { _streetsLoadingVisibility = value; NotifyOfPropertyChange(() => StreetsLoadingVisibility); }
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