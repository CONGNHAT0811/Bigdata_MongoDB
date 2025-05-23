using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrderQuanNet.DataManager;
using OrderQuanNet.Views;
using OrderQuanNet.Views.components.popup;


namespace OrderQuanNet
{
    public partial class Main : Window
    {
        public static Double LocationSaver = 0;

        public Action UpdateCartAction { get; set; }

        public static string HashMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes) { sb.Append(b.ToString("X2")); }
                return sb.ToString().ToLower();
            }
        }
        private enum TabType { Food, Drink, Time, OrdersManager, UserManager }
        private enum RightBarType { Orders, History }

        private TabType currentTab = TabType.Food;
        private RightBarType currentRightBar = RightBarType.Orders;

        public Main()
        {
            InitializeComponent();
            SetInitialContent();
            UpdateCartAction = () => { ReloadLayouts(null, null); };
        }
        private void SetInitialContent()
        {
            if (SessionManager.users == null)
            {
                this.Hide();

                Login login = new Login();
                login.ShowDialog();
                if (SessionManager.users == null || String.IsNullOrEmpty(SessionManager.users.type))
                {
                    Application.Current.Shutdown();
                    return;
                }
                this.Show();
            }

            UserCard.UserName = SessionManager.users.name;
            UserCard.UserType = SessionManager.users.type;
            this.WindowState = WindowState.Maximized;
            SwitchOrderOrHistory(OrderTab);
            ReloadLayouts(null, null);
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                currentRightBar = RightBarType.Orders;
                currentTab = TabType.Food;
                SessionManager.users = null;
                SetInitialContent();
            }
        }
        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đổi mật  khẩu  không ?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var resetPasswordWindow = new ResetPassword();
                resetPasswordWindow.ShowDialog();
            }
        }

        private void OrderToggle(object sender, EventArgs e) { SwitchRightBar(RightBarType.Orders); }
        private void HistoryToggle(object sender, EventArgs e) { SwitchRightBar(RightBarType.History); }
        private void FoodTab(object sender, EventArgs e) { SwitchTab(TabType.Food, new Food()); }
        private void DrinkTab(object sender, EventArgs e) { SwitchTab(TabType.Drink, new Drink()); }
        private void TimeTab(object sender, EventArgs e) { SwitchTab(TabType.Time, new Time()); }
        private void OrdersManagerTab(object sender, EventArgs e) { SwitchTab(TabType.OrdersManager, new OrdersManager()); }
        private void UserManagerTab(object sender, EventArgs e) { SwitchTab(TabType.UserManager, new UserManager()); }

        private void SwitchTab(TabType tabType, ContentControl content)
        {
            currentTab = tabType;
            ActiveTab(tabType);
            ReloadLayouts(null, null);
        }

        private void SwitchRightBar(RightBarType rightBarType)
        {
            currentRightBar = rightBarType;
            switch (currentRightBar)
            {
                case RightBarType.Orders:
                    RightBarManager.Content = new OrdersTab();
                    break;
                case RightBarType.History:
                    RightBarManager.Content = new HistoryTab();
                    break;
            }
            SwitchOrderOrHistory(currentRightBar == RightBarType.Orders ? OrderTab : HistoryTab);
        }

        private void ActiveTab(TabType tabType)
        {
            FoodTabItem.ItemActive = tabType == TabType.Food ? "true" : "false";
            DrinkTabItem.ItemActive = tabType == TabType.Drink ? "true" : "false";
            TimeTabItem.ItemActive = tabType == TabType.Time ? "true" : "false";
            AdminOrderTabItem.ItemActive = tabType == TabType.OrdersManager ? "true" : "false";
            AdminUserTabItem.ItemActive = tabType == TabType.UserManager ? "true" : "false";
        }

        private void SwitchOrderOrHistory(TextBlock tab)
        {
            OrderTab.Foreground = HistoryTab.Foreground = new SolidColorBrush(Colors.Black);
            tab.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void ReloadLayouts(object sender, EventArgs e)
        {
            if (SessionManager.users.type != "admin")
            {
                ADMIN_SHOWING_MANAGEMENT.Visibility = Visibility.Hidden;
                OrderAndHistory.Visibility = Visibility.Visible;
                ContentManager.Width = this.ActualWidth - 275 * 2;
            }
            else
            {
                ADMIN_SHOWING_MANAGEMENT.Visibility = Visibility.Visible;
                OrderAndHistory.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(ContentManager, 2);
                ContentManager.Width = this.ActualWidth - 275 - 20;
            }


            Sidebar.Height = this.ActualHeight - 50;
            OrderAndHistory.Height = this.ActualHeight - 50;

            TabControl.Height = this.ActualHeight - 55;
            RightBarManager.Height = this.ActualHeight - 100;

            Menu.Height = Sidebar.Height - 175;
            ContentManager.Height = this.ActualHeight - 55;

            switch (currentRightBar)
            {
                case RightBarType.Orders:
                    RightBarManager.Content = new OrdersTab();
                    break;
                case RightBarType.History:
                    RightBarManager.Content = new HistoryTab();
                    break;
            }

            switch (currentTab)
            {
                case TabType.Food:
                    ContentManager.Content = new Food();
                    break;
                case TabType.Drink:
                    ContentManager.Content = new Drink();
                    break;
                case TabType.Time:
                    ContentManager.Content = new Time();
                    break;
                case TabType.OrdersManager:
                    ContentManager.Content = new OrdersManager();
                    break;
                case TabType.UserManager:
                    ContentManager.Content = new UserManager();
                    break;
            }
        }
    }
}
