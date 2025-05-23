using System.Windows;
using System.Windows.Media.Imaging;
using OrderQuanNet.DataManager;
using OrderQuanNet.Models;

namespace OrderQuanNet.Views.components.popup
{
    public partial class Detail : Window
    {
        private Action _updateCart;

        private ProductsModel product;
        public Detail(int id)
        {
            InitializeComponent();
            product = loadProduct(id);
            if (product == null)
            {
                MessageBox.Show("Sản phẩm này không còn tồn tại!");
                this.Close();
                return;
            }

            Title.Text = product.name;
            Price.Text = "Giá: " + string.Format("{0:N0}", product.price);
            ImagePath.Source = new BitmapImage(new Uri(product.image_path, UriKind.RelativeOrAbsolute));

            _updateCart = ((Main)Application.Current.MainWindow).UpdateCartAction;
        }

        private ProductsModel loadProduct(int id)
        {
            var allProducts = ProductDataManager.Products;
            if (allProducts.Where(p => p.id == id).FirstOrDefault() != null)
                ProductDataManager.LoadProducts();
            return allProducts.Where(p => p.id == id).FirstOrDefault();
        }

        private void AddToCart(object sender, RoutedEventArgs e)
        {
            if (CartDataManager.getTotalItems() >= 10)
            {
                MessageBox.Show("Bạn chỉ có thể đặt tối đa 10 sản phẩm cùng lúc!");
                return;
            }
            int amouunt = int.Parse(QuantityTextBox.Text);
            CartDataManager.addItem((int)product.id, amouunt);
            MessageBox.Show("Đã thêm vào giỏ hàng!");
            _updateCart?.Invoke();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var fadeIn = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            this.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
