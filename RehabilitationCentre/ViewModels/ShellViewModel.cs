using Caliburn.Micro;
using RehabilitationCentre.ViewModels;

namespace RehabilitationCentre
{
    public class ShellViewModel : Conductor<object>, IShell
    {
        private object _selectedMenuItem;
        private BindableCollection<object> _menuItems = new BindableCollection<object>();

        public BindableCollection<object> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                NotifyOfPropertyChange(() => MenuItems);
            }
        }

        public object SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set
            {
                _selectedMenuItem = value;

                NotifyOfPropertyChange(() => SelectedMenuItem);
                NotifyOfPropertyChange(() => CurrentView);
                OpenNewView();
            }
        }

        protected override void OnInitialize()
        {
            MenuItems = new BindableCollection<object>()
            {
                new PacientsListViewModel()
            };

            base.OnInitialize();
        }

        public void OpenNewView()
        {
            if (SelectedMenuItem != null)
            {
                ActivateItem(SelectedMenuItem);
            }
        }

        public object CurrentView => SelectedMenuItem ?? null;
    }
}