using Castle.Windsor;
using Castle.Windsor.Installer;
using Core.Interfaces;
using Core.Types;
using RehabilitationCentre.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RehabilitationCentre.Models
{
    public class PacientsListModel
    {
        private PacientsListViewModel ViewModel;
        private static ICore Core;
        private static WindsorContainer _container;
        private DispatcherTimer Timer;

        public List<Pacient> PacientsList = new List<Pacient>();

        public PacientsListModel(PacientsListViewModel viewModel)
        {
            ViewModel = viewModel;

            _container = new WindsorContainer();

            _container.Install(FromAssembly.Named("Core"));

            Core = _container.Resolve<ICore>();

            Timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(10)
            };

            Timer.Tick += Timer_Tick;

            Timer.Start();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            await GetPacientsAsync();
        }

        public async Task<List<Pacient>> GetPacientsAsync()
        {
            try
            {
                var p = await Core.GetAllPacientsAsync();

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
            await Core.ShowPdfDocumentAsync(pacient);
        }

        public async Task GetDoctorsAsync()
        {
            var d = await Core.GetDoctorsAsync();

            ViewModel.Doctors.AddRange(d);
        }

        public async Task SetPacientVisitAsync(Pacient selectedPacient, Doctor selectedDoctor)
        {
            await Core.SetPacientVisit(selectedPacient, selectedDoctor);
        }
    }
}