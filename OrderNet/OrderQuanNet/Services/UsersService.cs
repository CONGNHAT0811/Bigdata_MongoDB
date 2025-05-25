using OrderQuanNet.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace OrderQuanNet.Services
{
    public class UsersService
    {
        private readonly Database<UsersModel> _database;

        public UsersService()
        {
            _database = new Database<UsersModel>("Users");
        }

        public void Insert(UsersModel user)
        {
            _database.Insert(user);
        }

        public bool Update(UsersModel user)
        {
            if (user._id == ObjectId.Empty)
                throw new ArgumentException("User id cannot be null or empty for update.");

            return _database.Update(user);
        }

        public bool Delete(UsersModel user)
        {
            if (user._id == ObjectId.Empty)
                throw new ArgumentException("User id cannot be null or empty for delete.");

            return _database.Delete((ObjectId)user._id);
        }

        public bool CheckOldPassword(ObjectId userId, string oldPassword)
        {
            var user = SelectById(userId);

            if (user != null && user.password == oldPassword)
                return true;

            return false;
        }

        public List<UsersModel> Select(UsersModel filterUser, bool includeNulls = false)
        {
            return _database.Select(filterUser, includeNulls);
        }

        public UsersModel? SelectById(ObjectId id)
        {
            return _database.SelectById(id);
        }

        public List<UsersModel> SelectAll()
        {
            return _database.SelectAll();
        }
    }
}
