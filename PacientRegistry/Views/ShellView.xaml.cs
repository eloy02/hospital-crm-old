using Microsoft.Win32;
using System.Windows;

namespace PacientRegistry
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
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