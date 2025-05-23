using OrderQuanNet.Models;

namespace OrderQuanNet.Services
{
    public class ProductsService
    {
        private readonly Database<ProductsModel> _database;
        public ProductsService() { _database = new Database<ProductsModel>("Products"); }

        public bool Insert(ProductsModel product) { return _database.Insert(product); }
        public bool Update(ProductsModel product) { return _database.Update(product); }
        public bool Delete(ProductsModel product) { return _database.Delete(product); }

        public ProductsModel Select(ProductsModel product)
        {
            using var reader = _database.Select(product);
            if (reader.Read())
            {
                return new ProductsModel
                {
                    id = Convert.ToInt32(reader["id"]),
                    name = reader["name"].ToString(),
                    type = reader["type"].ToString(),
                    price = Convert.ToInt32(reader["price"]),
                    image_path = reader["image_path"].ToString()
                };
            }
            return null;
        }

        public ProductsModel SelectById(int id)
        {
            using var reader = _database.SelectById(id);
            if (reader.Read())
            {
                return new ProductsModel
                {
                    id = Convert.ToInt32(reader["id"]),
                    name = reader["name"].ToString(),
                    type = reader["type"].ToString(),
                    price = Convert.ToInt32(reader["price"]),
                    image_path = reader["image_path"].ToString()
                };
            }
            return null;
        }

        public List<ProductsModel> SelectAll()
        {
            var products = new List<ProductsModel>();
            using var reader = _database.SelectAll();
            while (reader.Read())
            {
                products.Add(new ProductsModel
                {
                    id = Convert.ToInt32(reader["id"]),
                    name = reader["name"].ToString(),
                    type = reader["type"].ToString(),
                    price = Convert.ToInt32(reader["price"]),
                    image_path = reader["image_path"].ToString()
                });
            }

            return products;
        }
    }
}
