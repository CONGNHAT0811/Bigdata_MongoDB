using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using OrderQuanNet.DataManager;
using OrderQuanNet.Models;

namespace OrderQuanNet.Views.components.popup
{
    public partial class EditPopup : Window
    {
        private Action _updateCart;

        private ProductsModel product;

        public EditPopup(int id)
        {
            InitializeComponent();
            product = ProductDataManager.Products.Where(p => p.id == id).FirstOrDefault();

            if (product == null || product.id == null)
            {
                MessageBox.Show("Sản phẩm không còn tồn tại!");
                this.Close();
                return;
            }
            _updateCart = ((Main)Application.Current.MainWindow).UpdateCartAction;

            txtImageURL.Text = product.image_path;
            txtProductName.Text = product.name;
            txtPrice.Text = product.price.ToString();
            imgProduct.Source = new BitmapImage(new Uri(product.image_path, UriKind.Absolute));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            this.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string productName = txtProductName.Text;
            string price = txtPrice.Text;
            string imagePath = txtImageURL.Text;

            if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(price) || string.IsNullOrWhiteSpace(imagePath))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(price, out int priceValue))
            {
                MessageBox.Show("Giá tiền phải là số", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            product.price = int.Parse(price);
            product.image_path = imagePath;
            product.name = productName;
            product.save();
            ProductDataManager.UpdateProduct(product);

            _updateCart?.Invoke();
            MessageBox.Show("Đã cập nhật sản phẩm!");
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            product.delete();
            ProductDataManager.LoadProducts();
            MessageBox.Show("Đã xóa sản phẩm!");
            _updateCart?.Invoke();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void txtImageURL_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string imageUrl = txtImageURL.Text;
                if (!string.IsNullOrWhiteSpace(imageUrl))
                    imgProduct.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                else
                    imgProduct.Source = null;
            }
            catch (Exception)
            {
                imgProduct.Source = null;
                txtImageURL.Text = String.Empty;
            }
        }
    }
}
