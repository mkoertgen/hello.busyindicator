using System;
using System.Windows;
using Caliburn.Micro;

namespace hello.busyindicator
{
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BusyRibbonViewModel<TScreen> 
        : Screen, IBusyRibbon<TScreen>
        where TScreen : IScreen
    {
        public BusyRibbonViewModel(IEventAggregator events, TScreen busyScreen)
        {
            if (busyScreen == null) throw new ArgumentNullException(nameof(busyScreen));
            BusyScreen = new BusyViewModel<TScreen>(events, busyScreen);

            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            DisplayName = "hello.busyindicator";
        }

        public IBusyViewModel<TScreen> BusyScreen { get; }

        public void DoQuit() { TryClose(); }

        // ReSharper disable UnusedMember.Global
        public void DoGoBack() { ShowNotImplemented(); }
        public void DoGoForward() { ShowNotImplemented(); }
        public void DoShowHelp() { ShowNotImplemented(); }
        // ReSharper restore UnusedMember.Global

        private static void ShowNotImplemented() { MessageBox.Show("Not implemented"); }
    }
}