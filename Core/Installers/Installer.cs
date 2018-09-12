using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Core.Interfaces;

namespace Core.Installers
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Core.Init(container);
            container.Register(Component.For<ICore>().ImplementedBy<Core>());
        }
    }
}