using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MongoDB.Bson;
using OrderQuanNet.DataManager;

namespace OrderQuanNet.Views.components
{
    public partial class OrderItems : UserControl
    {
        private Action _updateCart;

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(string), typeof(OrderItems), new PropertyMetadata(string.Empty, OnItemIconChanged));

        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(
            "Amount", typeof(string), typeof(OrderItems), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemIdProperty = DependencyProperty.Register(
            "ItemId", typeof(string), typeof(OrderItems), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(
            "ItemName", typeof(string), typeof(OrderItems), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemPriceProperty = DependencyProperty.Register(
            "ItemPrice", typeof(string), typeof(OrderItems), new PropertyMetadata(string.Empty));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public string Amount
        {
            get { return (string)GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
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

        public OrderItems()
        {
            InitializeComponent();
            _updateCart = ((Main)Application.Current.MainWindow).UpdateCartAction;
        }

        private static void OnItemIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (OrderItems)d;
            var newIcon = e.NewValue as string;

            if (!string.IsNullOrEmpty(newIcon))
            {
                control.ItemImage.Source = new BitmapImage(new Uri(newIcon, UriKind.RelativeOrAbsolute));
            }
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            ObjectId id = ObjectId.Parse(ItemId);
            CartDataManager.removeItem(id);
            MessageBox.Show("Đã xóa sản phẩm khỏi giỏ hàng");
            _updateCart?.Invoke();
        }
    }
}
