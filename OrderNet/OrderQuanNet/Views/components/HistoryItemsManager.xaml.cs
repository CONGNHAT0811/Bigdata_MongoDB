using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OrderQuanNet.DataManager;
using OrderQuanNet.Models;
using OrderQuanNet.Services;

namespace OrderQuanNet.Views.components
{
    public partial class HistoryItemsManager : UserControl
    {
        private Action _updateCart;

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(string), typeof(HistoryItemsManager), new PropertyMetadata(string.Empty, OnItemIconChanged));

        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(
            "Amount", typeof(string), typeof(HistoryItemsManager), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            "Status", typeof(string), typeof(HistoryItemsManager), new PropertyMetadata(string.Empty, OnStatusChanged));

        public static readonly DependencyProperty ItemIdProperty = DependencyProperty.Register(
            "ItemId", typeof(string), typeof(HistoryItemsManager), new PropertyMetadata(string.Empty, OnItemIdChanged));

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(
            "ItemName", typeof(string), typeof(HistoryItemsManager), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ItemPriceProperty = DependencyProperty.Register(
            "ItemPrice", typeof(string), typeof(HistoryItemsManager), new PropertyMetadata(string.Empty));

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

        public HistoryItemsManager()
        {
            InitializeComponent();
            _updateCart = ((Main)Application.Current.MainWindow).UpdateCartAction;
        }

        private void ChangeOrdersStatus(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(ProccessBtn.Tag.ToString());

            OrdersService ordersService = new OrdersService();
            OrdersModel orders = ordersService.SelectById(id);

            if (orders != null)
            {
                switch (orders.status)
                {
                    case "WAITING":
                        orders.status = "PROCCESSING";
                        break;
                    case "PROCCESSING":
                        orders.status = "DONE";
                        break;
                }
                orders.save();
                _updateCart?.Invoke();
            }
        }

        private static void OnItemIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (HistoryItemsManager)d;
            var id = int.Parse(e.NewValue as string);


            OrdersService ordersService = new OrdersService();
            OrdersModel orders = ordersService.SelectById(id);

            if (orders != null)
            {
                UsersService usersService = new UsersService();
                UsersModel users = usersService.SelectById((int)orders.users_id);
                control.Username.Text = "By: " + users.name;
            }
        }

        private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (HistoryItemsManager)d;
            var status = e.NewValue as string;
            var statusBase = HistoryDataManager.StatusMappings.Where(x => x.Value == status).FirstOrDefault().Key;
            switch (statusBase)
            {
                case "WAITING":
                    control.Foreground = new SolidColorBrush(Colors.Orange);
                    control.ProccessBtn.Content = "Xác nhận";
                    control.ProccessBtn.Background = new SolidColorBrush(Colors.DarkOrange);
                    break;
                case "PROCCESSING":
                    control.Foreground = new SolidColorBrush(Colors.Coral);
                    control.ProccessBtn.Content = "Hoàn thành";
                    control.ProccessBtn.Background = new SolidColorBrush(Colors.ForestGreen);
                    break;
                case "DONE":
                    control.Foreground = new SolidColorBrush(Colors.Green);
                    control.ProccessBtn.Visibility = Visibility.Hidden;
                    break;
            }
        }


        private static void OnItemIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (HistoryItemsManager)d;
            var newIcon = e.NewValue as string;

            if (!string.IsNullOrEmpty(newIcon))
            {
                control.ItemImage.Source = new BitmapImage(new Uri(newIcon, UriKind.RelativeOrAbsolute));
            }
        }
    }
}
