using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace RehabilitationCentre.Views
{
    /// <summary>
    /// Логика взаимодействия для PacientInfoView.xaml
    /// </summary>
    public partial class PacientInfoView : UserControl
    {
        public PacientInfoView()
        {
            InitializeComponent();
        }

        private void ChoosePdfPath_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Pdf Files|*.pdf",
                Title = "Выберите PDF файлы документов ИПРА или ПМПК"
            };
            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
            }

            PdfPath.Text = filePath;
        }
    }
}