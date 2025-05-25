using System.Windows;
using System.Windows.Media.Animation;
using MongoDB.Bson;
using OrderQuanNet.DataManager;
using OrderQuanNet.Models;
using OrderQuanNet.Services;

namespace OrderQuanNet.Views.components.popup
{
    public partial class EditPopupUser : Window
    {
        private Action _updateCart;

        private UsersModel user;

        public EditPopupUser(string id)
        {
            InitializeComponent();

            UsersService usersService = new UsersService();
            user = usersService.SelectById(ObjectId.Parse(id.ToString()));

            if (user == null)
            {
                MessageBox.Show("Tài khoản này không tồn tại!");
                _updateCart?.Invoke();
                this.Close();
                return;
            }
            _updateCart = ((Main)Application.Current.MainWindow).UpdateCartAction;

            txtBalance.Text = user.balance.ToString();
            txtName.Text = user.name;
            txtUserName.Text = user.username;
            txtType.Text = user.type;

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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string userName = txtUserName.Text;
            string Name = txtName.Text;
            string balance = txtBalance.Text;
            string type = txtType.Text;

            if (string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(balance) ||
                string.IsNullOrWhiteSpace(type))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            user.balance = int.Parse(balance);
            user.name = Name;
            user.username = userName;
            user.type = type;
            user.save();
            _updateCart?.Invoke();
            UserDataManager.UpdateUser(user);
            MessageBox.Show("Đã cập nhật thông tin người dùng!");
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBox = MessageBox.Show("Xóa người dùng " + user.name + " ?", "Xóa người dùng", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (messageBox != MessageBoxResult.Yes)
            {
                try
                {
                    user.delete();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Vui lòng xóa các dữ liệu liên quan trước khi xóa users này!");
                }
                MessageBox.Show("Đã xóa người dùng! " + user.name);
                UserDataManager.LoadUsers();
                _updateCart?.Invoke();
                this.Close();
            }
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
