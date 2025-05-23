using System.Windows;
using System.Windows.Controls;
using OrderQuanNet.Views.components.popup;

namespace OrderQuanNet.Views.components
{
    public partial class UserItemsManager : UserControl
    {
        private Action _updateCart;

        public static readonly DependencyProperty FullNameProperty = DependencyProperty.Register(
            "FullName", typeof(string), typeof(UserItemsManager), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty BalanceProperty = DependencyProperty.Register(
            "Balance", typeof(string), typeof(UserItemsManager), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty UserIdProperty = DependencyProperty.Register(
            "UserId", typeof(string), typeof(UserItemsManager), new PropertyMetadata(string.Empty));

        public string FullName
        {
            get { return (string)GetValue(FullNameProperty); }
            set { SetValue(FullNameProperty, value); }
        }
        public string Balance
        {
            get { return (string)GetValue(BalanceProperty); }
            set { SetValue(BalanceProperty, value); }
        }
        public string UserId
        {
            get { return (string)GetValue(UserIdProperty); }
            set { SetValue(UserIdProperty, value); }
        }

        public UserItemsManager()
        {
            InitializeComponent();
        }

        private void EditPopupUserManager(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            EditPopupUser editWindow = new EditPopupUser(int.Parse(btn.Tag.ToString()));
            editWindow.ShowDialog();
        }
    }
}
