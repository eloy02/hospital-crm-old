using Caliburn.Micro;
using Core.Types;

namespace PacientRegistry.ViewModels
{
    public class PacientInfoViewModel : Conductor<object>
    {
        public PacientInfoViewModel(Pacient pacient)
        {
            this.Pacient = pacient;
        }

        private Pacient _pacient;

        public Pacient Pacient
        {
            get { return _pacient; }
            set { _pacient = value; NotifyOfPropertyChange(() => Pacient); }
        }
    }
}