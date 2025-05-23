using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using OrderQuanNet.DataManager;
using OrderQuanNet.Views.components.popup;

namespace OrderQuanNet.Views
{
    public partial class Drink : UserControl
    {
        private Action _updateCart;

        public Drink()
        {
            InitializeComponent();
            LoadDrinkProducts();
            _updateCart = ((Main)Application.Current.MainWindow).UpdateCartAction;
            DrinkPanel.ScrollToVerticalOffset(Main.LocationSaver);
        }
        private void DrinkPanel_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Main.LocationSaver = DrinkPanel.VerticalOffset;
        }

        private void LoadDrinkProducts()
        {
            if (ProductDataManager.Products.Count == 0) ProductDataManager.LoadProducts();
            var allProducts = ProductDataManager.Products;
            if (SearchBox.Text != "" && SearchBox.Text != null) allProducts = allProducts.Where(p => p.name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
            var drinkProducts = allProducts.Where(p => p.type == "drink");
            DrinkItemsControl.ItemsSource = drinkProducts.ToList();

            DrinkTitle.Text = "QUẦY NƯỚC: " + drinkProducts.Count();
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadDrinkProducts();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateRows();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateRows();
            if (SessionManager.users.type == "admin")
            {
                AddButton.Visibility = Visibility.Visible;
                ResetButton.Visibility = Visibility.Visible;
            }
            else
            {
                AddButton.Visibility = Visibility.Hidden;
                ResetButton.Visibility = Visibility.Visible;
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            Add addWindow = new Add("drink");
            addWindow.ShowDialog();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            ProductDataManager.LoadProducts();
            _updateCart?.Invoke();
        }

        private void UpdateRows()
        {
            double itemWidth = 160;
            int rowCount = (int)(this.ActualWidth / itemWidth);

            UniformGrid uniformGrid = FindUniformGrid(DrinkItemsControl);
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
    }
}
