using Castle.Windsor;
using Core.Interfaces;

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
    }
}