using OrderQuanNet.Models;

namespace OrderQuanNet.Services
{
    public class UsersService
    {
        private readonly Database<UsersModel> _database;
        public UsersService() { _database = new Database<UsersModel>("Users"); }

        public bool Insert(UsersModel user) { return _database.Insert(user); }
        public bool Update(UsersModel user) { return _database.Update(user); }
        public bool Delete(UsersModel user) { return _database.Delete(user); }

        public bool CheckOldPassword(int userId, string oldPassword)
        {
            UsersModel user = SelectById(userId);

            if (user != null && user.password == oldPassword)
            {
                return true; 
            }
            return false; 
        }
        public UsersModel Select(UsersModel user)
        {
            using var reader = _database.Select(user);
            if (reader.Read())
            {
                return new UsersModel
                {
                    id = Convert.ToInt32(reader["id"]),
                    name = reader["name"].ToString(),
                    username = reader["username"].ToString(),
                    password = reader["password"].ToString(),
                    type = reader["type"].ToString(),
                    balance = Convert.ToInt32(reader["balance"])
                };
            }
            return null;
        }

        public UsersModel SelectById(int id)
        {
            using var reader = _database.SelectById(id);
            if (reader.Read())
            {
                return new UsersModel
                {
                    id = Convert.ToInt32(reader["id"]),
                    name = reader["name"].ToString(),
                    username = reader["username"].ToString(),
                    password = reader["password"].ToString(),
                    type = reader["type"].ToString(),
                    balance = Convert.ToInt32(reader["balance"])
                };
            }
            return null;
        }

        public List<UsersModel> SelectAll()
        {
            var users = new List<UsersModel>();
            using var reader = _database.SelectAll();
            while (reader.Read())
            {
                users.Add(new UsersModel
                {
                    id = Convert.ToInt32(reader["id"]),
                    name = reader["name"].ToString(),
                    username = reader["username"].ToString(),
                    password = reader["password"].ToString(),
                    type = reader["type"].ToString(),
                    balance = Convert.ToInt32(reader["balance"])
                });
            }
            return users;
        }
    }
}
