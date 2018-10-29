using Caliburn.Micro;
using MaterialDesignThemes.Wpf;

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
    }
}