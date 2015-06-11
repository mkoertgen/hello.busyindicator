using System.Windows;
using Autofac;
using Caliburn.Micro.Autofac;

namespace hello.busyindicator
{
    public class AppBootstrapper : AutofacBootstrapper<AppViewModel>
    {
        public AppBootstrapper() { Initialize(); }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<AppViewModel>();
        }

        protected override void ConfigureBootstrapper()
        {
            base.ConfigureBootstrapper();
            EnforceNamespaceConvention = false;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<BusyViewModel>().As<IBusyViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<MainViewModel>().As<IMainViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<StartTaskViewModel>().As<IStartTaskViewModel>().InstancePerLifetimeScope();
        }
    }
}