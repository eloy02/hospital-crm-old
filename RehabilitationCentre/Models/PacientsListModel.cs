using Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using WebClient.Interfaces;

namespace RehabilitationCentre.Models
{
    public class PacientsListModel
    {
        public readonly IWebClient WebClient;
        public DispatcherTimer Timer;

        public List<Pacient> PacientsList = new List<Pacient>();

        public PacientsListModel(IWebClient webClient)
        {
            WebClient = webClient;

            Timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(10)
            };

            Timer.Tick += Timer_Tick;
        }

        public async Task GetProgrammTokenAsync()
        {
            await WebClient.GetProgrammTokenAsync();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            await GetPacientsAsync();
        }

        public async Task<List<Pacient>> GetPacientsAsync()
        {
            try
            {
                var p = await WebClient.GetPacientsAsync();

                if (p != null)
                {
                    PacientsList.Clear();
                    PacientsList.AddRange(p);

                    return p.ToList();
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task OpenPacientDocument(Pacient pacient)
        {
            var path = Directory.GetCurrentDirectory() + @"\Temp";
            var file = $"{path}\\{pacient.LastName}{pacient.FirstName}{pacient.PatronymicName}.pdf";

            if (File.Exists(file))
            {
                var pdfProc = System.Diagnostics.Process.Start(file);
            }
            else
            {
                var doc = await WebClient.GetPacientDocumentAsync(pacient);
                Directory.CreateDirectory(path);

                File.WriteAllBytes(file, doc.Content.ToArray());
                var pdfProc = System.Diagnostics.Process.Start(file);

                doc = null;
            }
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
        {
            var d = await WebClient.GetDoctorsAsync();

            if (d != null)
                return d;
            else return null;
        }

        public async Task<bool> SetPacientVisitAsync(Pacient selectedPacient, Doctor selectedDoctor)
        {
            var r = await WebClient.SavePacientVisitAsync(selectedPacient, selectedDoctor);

            return r;
        }

        public async Task DeleteToken()
        {
            await WebClient.DeleteToken();
        }

        public void ClearTempFolder()
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