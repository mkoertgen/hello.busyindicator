# Hello.BusyIndicator

A Wpf sample for using [Xceeds BusyIndicator](http://wpftoolkit.codeplex.com/wikipage?title=BusyIndicator&referringTitle=Home) in a decoupled, testable and [MVVM](http://en.wikipedia.org/wiki/Model_View_ViewModel)-friendly way with [Caliburn.Micro](http://caliburnmicro.com/) and [AutoFac](http://autofac.org/) using including support for cancellation and optional progress.

![Screenshot of sample application](HelloBusyIndicator.png)

## Usage
### BusyViewModel <-> BusyIndicator

Add an `IBusyViewModel` to your main view model and bind it to a [BusyIndicator](http://wpftoolkit.codeplex.com/wikipage?title=BusyIndicator&referringTitle=Home) in your view, e.g. in the view model

    public class AppViewModel : Screen, IAppViewModel
    {
        public IBusyViewModel BusyIndicator { get; }
 
        public AppViewModel(IBusyViewModel busyIndicator, ...)
        {
			...

and in the view

    ...
	<xctk:BusyIndicator IsBusy="{Binding BusyIndicator.IsBusy}">
        <xctk:BusyIndicator.BusyContent>
	       ...
        </xctk:BusyIndicator.BusyContent>

       <... main view goes here .. >

    </xctk:BusyIndicator>


Don't forget to register an `IBusyViewModel` implementation in the bootstrapper 

    protected override void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<BusyViewModel>()
			.As<IBusyViewModel>()
			.InstancePerLifetimeScope();
		...
    }

### Starting Tasks

To fire off a long running task from anywhere, publish a `StartTaskMessage` to the Caliburn `IEventAggregator` like this

        public void Start()
        {
            var message = new StartTaskMessage(MyLongRunningTask,
                $"Waiting for 'long running task'...");
            _events.PublishOnUIThread(message);
        }

        public bool CanStart => _events.HandlerExistsFor(typeof(StartTaskMessage));

        private static void MyLongRunningTask(CancellationToken token, IProgress<int> progress)
        {
            const int n = 20;
            var duration = TimeSpan.FromSeconds(20);

            for (int i = 1; i <= n; i++)
            {
                token.ThrowIfCancellationRequested();
                Thread.Sleep( (int)(duration.TotalMilliseconds / n) );
                progress?.Report((100 * i) / n);
            }
        }

### Notes on Ribbon

A [Ribbon](https://msdn.microsoft.com/en-us/library/ff799534%28v=vs.110%29.aspx) must be placed in a `RibbonWindow`. Wrapping a `Ribbon` inside a `BusyIndicator` pretty much works like expected, i.e. when busy the ribbon gets disabled. However, keytips and commands in views or tabs can still be triggered. You don't want the user starting another action while busy. So to prevent this also minimize & hide the ribbon like this

    <RibbonWindow.Resources>
        <busyindicator:BooleanToVisibilityConverter True="Hidden" False="Visible" x:Key="InverseBoolToVisible"/>
    </RibbonWindow.Resources>

	<...

    <Ribbon IsMinimized="{Binding BusyIndicator.IsBusy}"
        Visibility="{Binding BusyIndicator.IsBusy, Converter={StaticResource InverseBoolToVisible}}"
        Focusable="False" KeyboardNavigation.TabNavigation="Contained"
        WindowIconVisibility="Visible">
