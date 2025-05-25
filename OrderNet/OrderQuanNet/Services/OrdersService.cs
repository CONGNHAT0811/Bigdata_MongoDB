using MongoDB.Bson;
using OrderQuanNet.Models;
using System.Collections.Generic;

namespace OrderQuanNet.Services
{
    public class OrdersService
    {
        private readonly Database<OrdersModel> _database;

        public OrdersService()
        {
            _database = new Database<OrdersModel>("Orders");
        }

        public bool Insert(OrdersModel order)
        {
            return _database.Insert(order);
        }

        public bool Update(OrdersModel order)
        {
            if (order._id == ObjectId.Empty)
                return false;

            return _database.Update(order);
        }

        public bool Delete(OrdersModel order)
        {
            if (order._id == ObjectId.Empty)
                return false;
             return _database.Delete((ObjectId)(order._id));
        }

        public OrdersModel? SelectById(ObjectId id)
        {
            return _database.SelectById(id);
        }

        public List<OrdersModel> SelectAll(OrdersModel? ordersModel = null)
        {
            return _database.SelectAll(ordersModel);
        }

        public List<OrdersModel> Select(OrdersModel where)
        {
            return _database.Select(where);
        }
    }
}
