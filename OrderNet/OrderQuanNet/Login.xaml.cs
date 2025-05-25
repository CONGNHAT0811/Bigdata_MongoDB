using System.Windows;
using System.Windows.Input;
using OrderQuanNet.DataManager;
using OrderQuanNet.Models;
using OrderQuanNet.Services;

namespace OrderQuanNet
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!");
                return;
            }

            string hashedPassword = Main.HashMD5(password);

            UsersService usersService = new UsersService();
            UsersModel user = usersService.Select(new UsersModel { username = username, password = hashedPassword }).FirstOrDefault();

            if (user != null)
            {
                SessionManager.users = user;
                MessageBox.Show("Chào mừng trở lại! " + user.name);
                this.Close();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) LoginButton_Click(sender, e);
        }
    }
}
