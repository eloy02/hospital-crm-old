using Caliburn.Micro;
using Core.Types;
using RehabilitationCentre.Models;
using System.Threading.Tasks;
using WebClient.Interfaces;

namespace RehabilitationCentre.ViewModels
{
    public class VisitLogViewModel : Screen
    {
        private Pacient _pacient;
        private BindableCollection<VisitLog> _visitLogs = new BindableCollection<VisitLog>();
        private VisitLogModel Model;

        public BindableCollection<VisitLog> VisitLogs
        {
            get { return _visitLogs; }
            set { _visitLogs = value; NotifyOfPropertyChange(() => VisitLogs); }
        }

        public VisitLogViewModel(Pacient pacient, IWebClient webClient)
        {
            Model = new VisitLogModel(webClient);

            Pacient = pacient;
        }

        public Pacient Pacient
        {
            get { return _pacient; }
            set { _pacient = value; NotifyOfPropertyChange(() => Pacient); }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Task.Run(async () =>
            {
                if (Pacient != null)
                {
                    var v = await Model.GetVisistLogsForPacientAsync(Pacient);

                    if (v != null)
                    {
                        VisitLogs.AddRange(v);
                    }
                }
            });
        }
    }
}