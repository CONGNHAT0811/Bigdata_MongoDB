using System.Windows;
using System.Windows.Media.Animation;
using OrderQuanNet.DataManager;
using OrderQuanNet.Models;

namespace OrderQuanNet.Views.components.popup
{
    public partial class AddUser : Window
    {
        private Action _update;

        public AddUser(string v)
        {
            InitializeComponent();
            _update = ((Main)Application.Current.MainWindow).UpdateCartAction;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            this.BeginAnimation(OpacityProperty, fadeIn);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string userName = txtUserName.Text;
            string Name = txtName.Text;
            string balance = txtBalance.Text;
            string role = txtType.Text;

            if (string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(balance) ||
                string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UsersModel users = new UsersModel();
            users.balance = int.Parse(balance);
            users.name = Name;
            users.username = userName;
            users.type = role;
            users.password = Main.HashMD5(userName);
            users.create();

            UserDataManager.LoadUsers();
            MessageBox.Show("Đã cập nhật người dùng!");
            _update?.Invoke();

            this.Close();
        }
    }
}
