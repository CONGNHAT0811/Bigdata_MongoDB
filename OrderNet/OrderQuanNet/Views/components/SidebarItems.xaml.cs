using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrderQuanNet.Views.components
{
    public partial class SidebarItems : UserControl
    {
        public static readonly DependencyProperty ItemIconProperty = DependencyProperty.Register(
            "ItemIcon", typeof(string), typeof(SidebarItems), new PropertyMetadata(string.Empty, OnItemIconChanged));

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(
            "ItemName", typeof(string), typeof(SidebarItems), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemActiveProperty = DependencyProperty.Register(
            "ItemActive", typeof(string), typeof(SidebarItems), new PropertyMetadata("false", OnItemActiveChanged));

        public string ItemIcon
        {
            get { return (string)GetValue(ItemIconProperty); }
            set { SetValue(ItemIconProperty, value); }
        }

        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }

        public string ItemActive
        {
            get { return (string)GetValue(ItemActiveProperty); }
            set { SetValue(ItemActiveProperty, value); }
        }

        public SidebarItems()
        {
            InitializeComponent();
        }

        private static void OnItemIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SidebarItems)d;
            var newIcon = e.NewValue as string;

            if (!string.IsNullOrEmpty(newIcon))
            {
                control.Icon.Source = new BitmapImage(new Uri(newIcon, UriKind.RelativeOrAbsolute));
            }
        }

        private static void OnItemActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SidebarItems)d;
            var isActive = e.NewValue as string;

            if (!string.IsNullOrEmpty(isActive) && isActive == "true")
            {
                control.title.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c86f5"));
            }
            else
            {
                control.title.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
