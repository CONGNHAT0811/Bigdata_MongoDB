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
                    _id = (ObjectId)item._id,
                    amount = (int)item.amount,
                    status = item.status,
                    name = product?.name, 
                    image_path = product?.image_path,
                    price = product != null ? (int)product.price : 0, // Sửa ở đây
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
