using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using OrderQuanNet.DataManager;
using OrderQuanNet.Views.components.popup;

namespace OrderQuanNet.Views.components
{
    public partial class ProductItems : UserControl
    {
        private Action _updateCart;

        public static readonly DependencyProperty ItemIdProperty = DependencyProperty.Register(
            "ItemId", typeof(string), typeof(ProductItems), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(
            "ItemName", typeof(string), typeof(ProductItems), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemPriceProperty = DependencyProperty.Register(
            "ItemPrice", typeof(string), typeof(ProductItems), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ItemImage", typeof(string), typeof(ProductItems), new PropertyMetadata(string.Empty, OnItemIconChanged));

        public string ItemImage
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public string ItemId
        {
            get { return (string)GetValue(ItemIdProperty); }
            set { SetValue(ItemIdProperty, value); }
        }

        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }

        public string ItemPrice
        {
            get { return (string)GetValue(ItemPriceProperty); }
            set { SetValue(ItemPriceProperty, value); }
        }
        public ProductItems()
        {
            InitializeComponent();
            _updateCart = ((Main)Application.Current.MainWindow).UpdateCartAction;
        }

        private static void OnItemIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ProductItems)d;
            var newIcon = e.NewValue as string;

            if (!string.IsNullOrEmpty(newIcon)) control.ImageSource.Source = new BitmapImage(new Uri(newIcon, UriKind.RelativeOrAbsolute));
        }

        private void ItemClicked(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (SessionManager.users.type != "admin")
            {
                Detail detailWindow = new Detail(button.Tag.ToString());
                detailWindow.ShowDialog();
            }
            else
            {
                EditPopup editWindow = new EditPopup(button.Tag.ToString());
                editWindow.ShowDialog();
            }
        }
    }
}
