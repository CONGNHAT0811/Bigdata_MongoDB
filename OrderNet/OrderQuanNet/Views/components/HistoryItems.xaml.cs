using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OrderQuanNet.DataManager;

namespace OrderQuanNet.Views.components
{
    public partial class HistoryItems : UserControl
    {
        private Action _updateCart;

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(string), typeof(HistoryItems), new PropertyMetadata(string.Empty, OnItemIconChanged));

        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(
            "Amount", typeof(string), typeof(HistoryItems), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            "Status", typeof(string), typeof(HistoryItems), new PropertyMetadata(string.Empty, OnStatusChanged));

        public static readonly DependencyProperty ItemIdProperty = DependencyProperty.Register(
            "ItemId", typeof(string), typeof(HistoryItems), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(
            "ItemName", typeof(string), typeof(HistoryItems), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemPriceProperty = DependencyProperty.Register(
            "ItemPrice", typeof(string), typeof(HistoryItems), new PropertyMetadata(string.Empty));

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
        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public HistoryItems()
        {
            InitializeComponent();
        }

        private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (HistoryItems)d;
            var status = e.NewValue as string;
            var statusBase = HistoryDataManager.StatusMappings.Where(x => x.Value == status).FirstOrDefault().Key;
            switch (statusBase)
            {
                case "WAITING":
                    control.Foreground = new SolidColorBrush(Colors.Orange);
                    break;
                case "PROCCESSING":
                    control.Foreground = new SolidColorBrush(Colors.Coral);
                    break;
                case "DONE":
                    control.Foreground = new SolidColorBrush(Colors.Green);
                    break;
            }
        }

        private static void OnItemIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (HistoryItems)d;
            var newIcon = e.NewValue as string;

            if (!string.IsNullOrEmpty(newIcon))
            {
                control.ItemImage.Source = new BitmapImage(new Uri(newIcon, UriKind.RelativeOrAbsolute));
            }
        }

    }
}
