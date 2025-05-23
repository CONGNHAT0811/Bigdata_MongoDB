using OrderQuanNet.Models;

namespace OrderQuanNet.DataManager
{
    public static class SessionManager
    {
        private static UsersModel _users;

        public static UsersModel users
        {
            get { return _users; }
            set { _users = value; }
        }
    }
}
