using System.Windows;
using System.Windows.Controls;

namespace OrderQuanNet.Views.components
{
    public partial class UserCard : UserControl
    {
        public static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(
            "UserName", typeof(string), typeof(UserCard), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty UserTypeProperty = DependencyProperty.Register(
            "UserType", typeof(string), typeof(UserCard), new PropertyMetadata(string.Empty));

        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        public string UserType
        {
            get { return (string)GetValue(UserTypeProperty); }
            set { SetValue(UserTypeProperty, value); }
        }

        public UserCard()
        {
            InitializeComponent();
            today.Text = "Today: " + DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
}
