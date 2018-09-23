using System.Windows;

namespace RehabilitationCentre.Views
{
    /// <summary>
    /// Логика взаимодействия для PacientsList.xaml
    /// </summary>
    public partial class PacientsListView : Window
    {
        public PacientsListView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = "0123456789+".IndexOf(e.Text) < 0;
        }
    }
}