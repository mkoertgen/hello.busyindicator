using System.Windows;
using Autofac;
using Caliburn.Micro.Autofac;

namespace hello.busyindicator
{
    public class AppBootstrapper : AutofacBootstrapper<IBusyRibbon<IAppViewModel>>
    {
        public AppBootstrapper() { Initialize(); }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IBusyRibbon<IAppViewModel>>();
        }

        protected override void ConfigureBootstrapper()
        {
            base.ConfigureBootstrapper();
            EnforceNamespaceConvention = false;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<BusyRibbonViewModel<IAppViewModel>>().As<IBusyRibbon<IAppViewModel>>().InstancePerLifetimeScope();
            builder.RegisterType<AppViewModel>().As<IAppViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<NotificationsViewModel>().As<INotificationsViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<StartTaskViewModel>().As<IStartTaskViewModel>().InstancePerLifetimeScope();
        }
    }
}