using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using OrderQuanNet.DataManager;
using OrderQuanNet.Views.components.popup;

namespace OrderQuanNet.Views
{
    public partial class UserManager : UserControl
    {
        public UserManager()
        {
            InitializeComponent();
            loadUsers();
        }

        private void loadUsers()
        {
            if (UserDataManager.Users.Count == 0) UserDataManager.LoadUsers();
            var allUsers = UserDataManager.Users;
            if (SearchBox.Text != "" && SearchBox.Text != null) allUsers = allUsers.Where(p => p.name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
            UsersItemsControl.ItemsSource = allUsers;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadUsers();
        }

        private void UserManagerControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateRows();
        }

        private void UserManagerControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateRows();
        }

        private void EditPopupUserManager(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Tag != null)
            {
                EditPopupUser editWindow = new EditPopupUser(button.Tag.ToString());
                editWindow.ShowDialog();
            }
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            components.popup.Download downloadWindow = new components.popup.Download();
            downloadWindow.ShowDialog();
        }

        private void AddUserManager(object sender, RoutedEventArgs e)
        {
            AddUser addWindow = new AddUser("user");
            addWindow.ShowDialog();
        }

        private void UpdateRows()
        {
            double itemWidth = 160;
            int rowCount = (int)(this.ActualWidth / itemWidth);

            UniformGrid uniformGrid = FindUniformGrid(UserManagerPanel);
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
