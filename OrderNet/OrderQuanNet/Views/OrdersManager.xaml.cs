using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MongoDB.Bson;
using OrderQuanNet.DataManager;
using OrderQuanNet.Models;
using OrderQuanNet.Views.components.popup;

namespace OrderQuanNet.Views
{
    public partial class OrdersManager : UserControl
    {
        public OrdersManager()
        {
            InitializeComponent();
            loadOrders();
            OrdersPanel.ScrollToVerticalOffset(Main.LocationSaver);
        }
        private void OrdersPanel_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Main.LocationSaver = OrdersPanel.VerticalOffset;
        }

        private void loadOrders()
        {
            HistoryDataManager.LoadHistory();
            List<OrdersModel> history = HistoryDataManager.OrdersHistory;
            List<HistoryItem> items = new List<HistoryItem>();
            foreach (var item in history)
            {
                ProductsModel product = ProductDataManager.Products.Where(p => p._id == item.product_id).FirstOrDefault();
                items.Add(new HistoryItem
                {
                    id = item._id ?? ObjectId.Empty, // Fix for CS0029  
                    amount = item.amount ?? 0, // Fix for CS8629  
                    status = item.status ?? "Unknown", // Fix for CS8601  
                    name = product?.name ?? "Unknown Product", // Fix for CS8602 and CS8601  
                    image_path = product?.image_path ?? string.Empty, // Fix for CS8601  
                    price = product?.price ?? 0, // Fix for CS8629  
                });
            }
            OrderItemsControl.ItemsSource = items;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateRows();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateRows();
        }

        private void UpdateRows()
        {
            double itemWidth = 235;
            int rowCount = (int)(this.ActualWidth / itemWidth);

            UniformGrid uniformGrid = FindUniformGrid(OrderItemsControl);
            if (uniformGrid != null)
            {
                uniformGrid.Columns = rowCount;
                uniformGrid.InvalidateMeasure();
            }
        }

        private UniformGrid FindUniformGrid(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is UniformGrid uniformGrid) return uniformGrid;

                var foundChild = FindUniformGrid(child);
                if (foundChild != null) return foundChild;
            }
            return null;
        }
        private void Bill_Click(object sender, RoutedEventArgs e)
        {
            Bill billWindow = new Bill();
            billWindow.ShowDialog();
        }
    }
}
