# Hello.BusyIndicator

A Wpf sample for using [Xceeds BusyIndicator](http://wpftoolkit.codeplex.com/wikipage?title=BusyIndicator&referringTitle=Home) in a decoupled, testable and [MVVM](http://en.wikipedia.org/wiki/Model_View_ViewModel)-friendly way with [Caliburn.Micro](http://caliburnmicro.com/), [AutoFac](http://autofac.org/) using including support for cancellation and optional progress. The sample app also shows how to deal smoothly with the [Windows Ribbon](https://msdn.microsoft.com/en-us/library/ff799534%28v=vs.110%29.aspx) which in general does not play well with MVVM.

![Screenshot of sample application](HelloBusyIndicator.png)

## Usage

For the standard use case just wrap your main screen with a busy indicator. Let's say your main screen is an implementation of `IAppViewModel` and this is the main screen you set up in the bootstrapper, i.e.

    public class AppBootstrapper : AutofacBootstrapper<IAppViewModel>>
    {
		...
	    protected override void OnStartup(object sender, StartupEventArgs e)
	    {
	        DisplayRootViewFor<IAppViewModel>();
	    }

Then just wrap it into a `IBusyViewModel<>` like this

    protected override void OnStartup(object sender, StartupEventArgs e)
    {
        DisplayRootViewFor<IBusyViewModel<IAppViewModel>>();
    }

and additionaly register an implementation 

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<BusyViewModel<IAppViewModel>>()
				.As<IBusyViewModel<IAppViewModel>>()
				.InstancePerLifetimeScope();
			...
        }
    }



TODO...
## Ribbon

For some reason the Ribbon always needs to  
are showing. wa busy indicator around your main screen into BusyViewModel
