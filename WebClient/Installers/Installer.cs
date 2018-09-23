using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using WebClient.Interfaces;

namespace WebClient.Installers
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            WebClientApi.Init(container);
            container.Register(Component.For<IWebClient>().ImplementedBy<WebClientApi>());
        }
    }
}