using MaterialDesignThemes.Wpf;

namespace RehabilitationCentre.ViewModels
{
    public class PacientsListViewModel
    {
        public static object Icon
        {
            get { return PackIconKind.ViewList; }
        }

        public static string Name
        {
            get { return "Список пациентов"; }
        }
    }
}