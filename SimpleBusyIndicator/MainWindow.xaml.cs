using System.Windows;

namespace SimpleBusyIndicator
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleBusy_Click(object sender, RoutedEventArgs e)
        {
            var isBusy = !BusyIndicator.IsBusy;
            BusyIndicator.IsBusy = myBusyIndicator.IsBusy = isBusy;

            var visibility = isBusy ? Visibility.Visible : Visibility.Collapsed;
            BusyRectangle.Visibility = BusyPresenter.Visibility = visibility;
        }
    }
}
