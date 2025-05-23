using OrderQuanNet.Models;
using OrderQuanNet.Services;

namespace OrderQuanNet.DataManager
{
    public static class UserDataManager
    {
        private static List<UsersModel> _Users = new List<UsersModel>();

        public static List<UsersModel> Users
        {
            get { return _Users; }
            set { _Users = value; }
        }

        public static void LoadUsers()
        {
            UsersService usersService = new UsersService();
            UserDataManager.Users = usersService.SelectAll();
        }

        public static void UpdateUser(UsersModel User)
        {
            if (User == null) throw new ArgumentNullException(nameof(User), "User cannot be null.");
            var id = _Users.FindIndex(p => p.id == User.id);
            if (id != -1) _Users[id] = User;
        }
    }
}
