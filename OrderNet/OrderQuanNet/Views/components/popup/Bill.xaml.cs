using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using OrderQuanNet.Models;
using OrderQuanNet.Services;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using MongoDB.Bson;

namespace OrderQuanNet.Views.components.popup
{
    public partial class Bill : Window, INotifyPropertyChanged
    {
        private readonly UsersService _usersService;
        private readonly OrdersService _ordersService;
        private readonly ProductsService _productsService;

        public ObservableCollection<UsersModel> Users { get; set; }
        public ObservableCollection<OrderViewModel> SelectedUserOrders { get; set; }
        public UsersModel SelectedUser { get; set; }

        private decimal _grandTotal;
        public decimal GrandTotal
        {
            get { return _grandTotal; }
            set
            {
                if (_grandTotal != value)
                {
                    _grandTotal = value;
                    OnPropertyChanged(nameof(GrandTotal));  
                }
            }
        }

        public string CurrentDate { get; set; }
        public string InvoiceCode { get; set; }

        public Bill()
        {
            InitializeComponent();
            _usersService = new UsersService();
            _ordersService = new OrdersService();
            _productsService = new ProductsService();

            Users = new ObservableCollection<UsersModel>(_usersService.SelectAll().Where(user => user.type == "member"));
            CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            GenerateInvoiceCode();
            SelectedUserOrders = new ObservableCollection<OrderViewModel>();
            DataContext = this;  
        }
        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SelectedUser != null)
            {
                LoadUserOrders();
                CalculateGrandTotal();
            }
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
        private void LoadUserOrders()
        {
            var today = DateTime.Now.Date;
            var orders = _ordersService
                .SelectAll(new OrdersModel { users_id = SelectedUser._id })
                .Where(order =>
                    order.status == "DONE" &&
                    DateTime.TryParse(order.updated_at, out var updatedDate) &&
                    updatedDate.Date == today); 

            SelectedUserOrders.Clear();

            foreach (var order in orders)
            {
                string productName = GetProductNameById(order.product_id ?? ObjectId.Empty);

                SelectedUserOrders.Add(new OrderViewModel
                {
                    ProductName = productName,
                    Quantity = order.amount ?? 0,
                    Price = order.price ?? 0m,
                    Total = (order.amount ?? 0) * (order.price ?? 0m)
                });
            }
            CalculateGrandTotal();
            OnPropertyChanged(nameof(SelectedUserOrders));
        }

        private string GetProductNameById(ObjectId productId)
        {
            var product = _productsService.SelectById(productId);
            return product?.name ?? "Unknown Product";
        }

        private void CalculateGrandTotal()
        {
            GrandTotal = SelectedUserOrders?.Sum(order => order.Total) ?? 0;
            OnPropertyChanged(nameof(GrandTotal)); 
        }

        private void GenerateInvoiceCode()
        {
            var random = new Random();
            InvoiceCode = $"#{random.Next(100000, 999999)}";
            OnPropertyChanged(nameof(InvoiceCode));
        }


        private void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"Invoice_{InvoiceCode}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                ExportButton.Visibility = Visibility.Collapsed;
                CloseButton.Visibility = Visibility.Collapsed;

                var bitmapSource = RenderToBitmap(BillGrid);

                ExportButton.Visibility = Visibility.Visible;
                CloseButton.Visibility = Visibility.Visible;

                var pdf = new PdfDocument();
                var page = pdf.AddPage();
                var gfx = XGraphics.FromPdfPage(page);

                using (var memoryStream = new MemoryStream())
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(memoryStream);

                    var xImage = XImage.FromStream(memoryStream);

                    double imgWidth = bitmapSource.PixelWidth;
                    double imgHeight = bitmapSource.PixelHeight;
                    double pdfWidth = page.Width;
                    double pdfHeight = page.Height;

                    double scaleX = pdfWidth / imgWidth;
                    double scaleY = pdfHeight / imgHeight;
                    double scale = Math.Min(scaleX, scaleY);  

                    double renderWidth = imgWidth * scale;
                    double renderHeight = imgHeight * scale;

                    gfx.DrawImage(xImage,
                                  (pdfWidth - renderWidth) / 2,  
                                  (pdfHeight - renderHeight) / 2,  
                                  renderWidth,
                                  renderHeight);
                }

                pdf.Save(filePath);
                MessageBox.Show($"Invoice saved to {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private BitmapSource RenderToBitmap(UIElement element)
        {
            var width = (int)element.RenderSize.Width;
            var height = (int)element.RenderSize.Height;

            var renderTargetBitmap = new RenderTargetBitmap(
                width,
                height, 
                96,  
                96,  
                PixelFormats.Pbgra32);
            renderTargetBitmap.Render(element);
            return renderTargetBitmap;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

    public class OrderViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}
