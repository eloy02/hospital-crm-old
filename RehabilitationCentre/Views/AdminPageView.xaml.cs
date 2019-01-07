using System.Windows.Controls;

namespace RehabilitationCentre.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminPageView.xaml
    /// </summary>
    public partial class AdminPageView : UserControl
    {
        public AdminPageView()
        {
            InitializeComponent();
        }

        private void ListViewScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}