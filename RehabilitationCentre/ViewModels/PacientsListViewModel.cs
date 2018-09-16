using Caliburn.Micro;
using Core.Types;
using MaterialDesignThemes.Wpf;
using RehabilitationCentre.Models;

namespace RehabilitationCentre.ViewModels
{
    public class PacientsListViewModel : Screen
    {
        private BindableCollection<Pacient> pacients = new BindableCollection<Pacient>();
        private bool _chooseDoctor = false;
        private PacientsListModel Model;
        private bool _isPacientsLoading = false;
        private Pacient _selectedPacient;

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

        protected override void OnInitialize()
        {
            Model = new PacientsListModel(this);

            base.OnInitialize();
        }

        protected override async void OnActivate()
        {
            IsPacientsLoading = true;

            await Model.GetPacientsAsync();

            IsPacientsLoading = false;
        }

        public async void ShowPacientDocumet()
        {
            if (SelectedPacient != null)
            {
                await Model.OpenPacientDocument(SelectedPacient);
            }
        }
    }
}