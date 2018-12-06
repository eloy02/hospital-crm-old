using System.ComponentModel;
using System.IO;
using System.Windows;

namespace RehabilitationCentre.Views
{
    /// <summary>
    /// Логика взаимодействия для PacientsList.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var path = Directory.GetCurrentDirectory() + @"\Temp";
            DirectoryInfo di = new DirectoryInfo(path);

            if (di.Exists)
            {
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }
            }
        }
    }
}