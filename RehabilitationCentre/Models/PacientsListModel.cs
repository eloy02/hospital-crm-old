using Castle.Windsor;
using Castle.Windsor.Installer;
using Core.Types;
using RehabilitationCentre.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using WebClient;

namespace RehabilitationCentre.Models
{
    public class PacientsListModel
    {
        private PacientsListViewModel ViewModel;
        private WebClientApi WebClientApi = new WebClientApi();
        public Guid WebToken;

        private static WindsorContainer _container;
        private DispatcherTimer Timer;

        public List<Pacient> PacientsList = new List<Pacient>();

        public PacientsListModel(PacientsListViewModel viewModel)
        {
            ViewModel = viewModel;

            _container = new WindsorContainer();

            _container.Install(FromAssembly.Named("Core"));

            Timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(10)
            };

            Timer.Tick += Timer_Tick;

            Timer.Start();
        }

        public async Task GetProgrammTokenAsync()
        {
            WebToken = await WebClientApi.GetProgrammTokenAsync();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            await GetPacientsAsync();
        }

        public async Task<List<Pacient>> GetPacientsAsync()
        {
            try
            {
                var p = await WebClientApi.GetPacientsAsync(WebToken);

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
            var doc = await WebClientApi.GetPacientDocumentAsync(WebToken, pacient);

            var path = Directory.GetCurrentDirectory() + @"\Temp";
            Directory.CreateDirectory(path);
            var file = $"{path}\\{Guid.NewGuid().ToString()}.pdf";
            File.WriteAllBytes(file, doc.Content.ToArray());
            var pdfProc = System.Diagnostics.Process.Start(file);
        }

        public async Task GetDoctorsAsync()
        {
            var d = await WebClientApi.GetDoctorsAsync(WebToken);

            if (d != null)
                ViewModel.Doctors.AddRange(d);
        }

        public async Task SetPacientVisitAsync(Pacient selectedPacient, Doctor selectedDoctor)
        {
            await WebClientApi.SavePacientVisitAsync(WebToken, selectedPacient, selectedDoctor);
        }

        public async Task DeleteToken()
        {
            await WebClientApi.DeleteToken(WebToken);
        }
    }
}