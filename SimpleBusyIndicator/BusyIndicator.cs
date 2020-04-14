using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SimpleBusyIndicator
{
    [StyleTypedProperty(Property = "OverlayStyle", StyleTargetType = typeof(Rectangle))]
    public class BusyIndicator : ContentControl
    {
        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator), new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            nameof(IsBusy), typeof(bool), typeof(BusyIndicator));
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            nameof(BusyContent), typeof(object), typeof(BusyIndicator));
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            nameof(OverlayStyle), typeof(Style), typeof(BusyIndicator));

        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof(DataTemplate),
            typeof(BusyIndicator));

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        public Style OverlayStyle
        {
            get => (Style)GetValue(OverlayStyleProperty);
            set => SetValue(OverlayStyleProperty, value);
        }

        public object BusyContent
        {
            get => GetValue(BusyContentProperty);
            set => SetValue(BusyContentProperty, value);
        }

        public DataTemplate BusyContentTemplate
        {
            get => (DataTemplate)GetValue(BusyContentTemplateProperty);
            set => SetValue(BusyContentTemplateProperty, value);
        }
    }
}
