using Castle.Windsor;
using Castle.Windsor.Installer;
using Core.Interfaces;
using Core.Types;
using RehabilitationCentre.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabilitationCentre.Models
{
    public class PacientsListModel
    {
        private PacientsListViewModel ViewModel;
        private static ICore Core;
        private static WindsorContainer _container;

        public PacientsListModel(PacientsListViewModel viewModel)
        {
            ViewModel = viewModel;

            _container = new WindsorContainer();

            _container.Install(FromAssembly.Named("Core"));

            Core = _container.Resolve<ICore>();
        }

        public async Task<List<Pacient>> GetPacientsAsync()
        {
            try
            {
                var p = (await Core.GetAllPacientsAsync()).ToList();

                return p;
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