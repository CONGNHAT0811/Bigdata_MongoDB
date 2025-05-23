using OrderQuanNet.Models;

namespace OrderQuanNet.Services
{
    public class OrdersService
    {
        private readonly Database<OrdersModel> _database;
        public OrdersService() { _database = new Database<OrdersModel>("Orders"); }

        public bool Insert(OrdersModel order) { return _database.Insert(order); }
        public bool Update(OrdersModel order) { return _database.Update(order); }
        public bool Delete(OrdersModel order) { return _database.Delete(order); }

        public OrdersModel Select(OrdersModel order)
        {
            using var reader = _database.Select(order);
            if (reader.Read())
            {
                return new OrdersModel
                {
                    id = Convert.ToInt32(reader["id"]),
                    users_id = Convert.ToInt32(reader["users_id"]),
                    product_id = Convert.ToInt32(reader["product_id"]),
                    amount = Convert.ToInt32(reader["amount"]),
                    price = Convert.ToInt32(reader["price"]),
                    status = reader["status"].ToString(),
                    created_at = reader["created_at"].ToString(),
                    updated_at = reader["updated_at"].ToString()
                };
            }
            return null;
        }

        public OrdersModel SelectById(int id)
        {
            using var reader = _database.SelectById(id);
            if (reader.Read())
            {
                return new OrdersModel
                {
                    id = Convert.ToInt32(reader["id"]),
                    users_id = Convert.ToInt32(reader["users_id"]),
                    product_id = Convert.ToInt32(reader["product_id"]),
                    amount = Convert.ToInt32(reader["amount"]),
                    price = Convert.ToInt32(reader["price"]),
                    status = reader["status"].ToString(),
                    created_at = reader["created_at"].ToString(),
                    updated_at = reader["updated_at"].ToString()
                };
            }
            return null;
        }

        public List<OrdersModel> SelectAll(OrdersModel where = null)
        {
            var orders = new List<OrdersModel>();
            using var reader = _database.SelectAll(where);
            while (reader.Read())
            {
                orders.Add(new OrdersModel
                {
                    id = Convert.ToInt32(reader["id"]),
                    users_id = Convert.ToInt32(reader["users_id"]),
                    product_id = Convert.ToInt32(reader["product_id"]),
                    amount = Convert.ToInt32(reader["amount"]),
                    price = Convert.ToInt32(reader["price"]),
                    status = reader["status"].ToString(),
                    created_at = reader["created_at"].ToString(),
                    updated_at = reader["updated_at"].ToString()
                });
            }
            return orders;
        }
    }
}
