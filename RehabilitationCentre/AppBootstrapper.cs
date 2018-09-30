namespace RehabilitationCentre
{
    using Caliburn.Micro;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using RehabilitationCentre.Models;
    using RehabilitationCentre.ViewModels;
    using System;
    using System.Collections.Generic;

    public class AppBootstrapper : BootstrapperBase
    {
        private readonly IWindsorContainer container = new WindsorContainer();

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            container.Register(Component.For<IShell>().ImplementedBy<PacientsListViewModel>());
            container.Register(Component.For<PacientsListModel>());

            container.Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>());
            container.Register(Component.For<IEventAggregator>().ImplementedBy<EventAggregator>());

            container.Install(FromAssembly.Named("WebClient"));
        }

        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrWhiteSpace(key)

                ? container.Kernel.HasComponent(service)
                    ? container.Resolve(service)
                    : base.GetInstance(service, key)

                : container.Kernel.HasComponent(key)
                    ? container.Resolve(key, service)
                    : base.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.Kernel.HasComponent(service)
                    ? (object[])container.ResolveAll(service)
                    : base.GetAllInstances(service);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}