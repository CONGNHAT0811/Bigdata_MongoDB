using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using OrderQuanNet.DataManager;
using OrderQuanNet.Models;

namespace OrderQuanNet.Views.components.popup
{
    public partial class Add : Window
    {
        private Action _updateCart;
        private string _type;
        public Add(string type)
        {
            InitializeComponent();
            _type = type.ToLower();
            _updateCart = ((Main)Application.Current.MainWindow).UpdateCartAction;
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

        private void txtImagePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string imageUrl = txtImagePath.Text;
                if (!string.IsNullOrWhiteSpace(imageUrl))
                    imgProduct.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                else
                    imgProduct.Source = null;
            }
            catch (Exception)
            {
                imgProduct.Source = null;
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string productName = txtProductName.Text;
            string price = txtPrice.Text;
            string imagePath = txtImagePath.Text;

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

            ProductsModel product = new ProductsModel();
            product.price = priceValue;
            product.name = productName;
            product.image_path = imagePath;
            product.type = _type;

            product.create();
            ProductDataManager.LoadProducts();
            _updateCart?.Invoke();
            MessageBox.Show("Đã cập nhật sản phẩm!");
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
