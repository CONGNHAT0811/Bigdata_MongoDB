using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using OrderQuanNet.DataManager;

namespace OrderQuanNet.Views.components.popup
{
    public partial class Download : Window
    {
        public Download()
        {
            InitializeComponent();
            UserDataManager.LoadUsers();
            ProductsDataGrid.ItemsSource = UserDataManager.Users;
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
        private void ExportToCSV(object sender, RoutedEventArgs e)
        {
            var users = UserDataManager.Users;
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                FileName = "users_data.csv"        
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                var filePath = saveFileDialog.FileName;
                var csvContent = new StringBuilder();
                csvContent.AppendLine("ID,Username,Name,Type,Balance");
                foreach (var user in users)
                {
                    csvContent.AppendLine($"{user._id},{user.username},{user.name},{user.type},{user.balance}");
                }
                File.WriteAllText(filePath, csvContent.ToString());
                MessageBox.Show("Đã xuất dữ liệu ra CSV!");
                this.Close();
                return;
            }
            MessageBox.Show("Chưa chọn tệp để lưu.");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
