using System;
using System.Windows;
using System.Security.Cryptography;
using System.Text; 
using OrderQuanNet.Services;
using OrderQuanNet.Models;
using OrderQuanNet.DataManager;

namespace OrderQuanNet.Views.components.popup
{
    public partial class ResetPassword : Window
    {
        private UsersModel currentUser;

        public ResetPassword()
        {
            InitializeComponent();
            currentUser = SessionManager.users; 
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = OldPasswordBox.Password;
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            string hashedOldPassword = Main.HashMD5(oldPassword);

            if (currentUser.password != hashedOldPassword)
            {
                MessageBox.Show("Mật khẩu cũ không đúng.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận khác nhau.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (newPassword.Length < 8)
            {
                MessageBox.Show("Mật khẩu mới phải dài ít nhất 8 ký tự.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string hashedNewPassword = Main.HashMD5(newPassword);
            currentUser.password = hashedNewPassword;
            var userService = new UsersService();
            bool updateSuccess = userService.Update(currentUser);

            if (updateSuccess)
            {
                MessageBox.Show("Mật khẩu đã được cập nhật thành công.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Đã có lỗi xảy ra khi cập nhật mật khẩu. Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
