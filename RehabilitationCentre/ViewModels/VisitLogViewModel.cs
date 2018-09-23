using Caliburn.Micro;
using Core.Types;
using RehabilitationCentre.Models;
using System;
using System.Threading.Tasks;

namespace RehabilitationCentre.ViewModels
{
    public class VisitLogViewModel : Conductor<object>
    {
        private Pacient _pacient;
        private BindableCollection<VisitLog> _visitLogs = new BindableCollection<VisitLog>();
        private VisitLogModel Model = new VisitLogModel();
        private Guid Token;

        public BindableCollection<VisitLog> VisitLogs
        {
            get { return _visitLogs; }
            set { _visitLogs = value; NotifyOfPropertyChange(() => VisitLogs); }
        }

        public VisitLogViewModel(Pacient pacient, Guid token)
        {
            Pacient = pacient;

            this.Token = token;
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
                Model.WebToken = Token;

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