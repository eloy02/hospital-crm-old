using Castle.Windsor;
using Core.Interfaces;
using Core.Types;
using System.Threading.Tasks;

namespace Core
{
    public class Core : ICore
    {
        private static IWindsorContainer _container;

        public static void Init(IWindsorContainer ioc)
        {
            _container = ioc;
            //Log = _container.Resolve<ILogger>();
        }

        public async Task SavePacientAsync(PacientCore pacient)
        {
        }
    }
}