using OrderQuanNet.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace OrderQuanNet.Services
{
    public class ProductsService
    {
        private readonly Database<ProductsModel> _database;

        public ProductsService()
        {
            _database = new Database<ProductsModel>("Products");
        }

        public void Insert(ProductsModel product)
        {
            _database.Insert(product);
        }

        public void Update(ProductsModel product)
        {
            _database.Update(product);
        }

        public bool Delete(ProductsModel product)
        {
            if (product._id == ObjectId.Empty)
                throw new ArgumentException("Product Id cannot be empty for delete.");

            return _database.Delete((ObjectId)product._id);
        }

        public List<ProductsModel> Select(ProductsModel filterProduct, bool includeNulls = false)
        {
            return _database.Select(filterProduct, includeNulls);
        }

        public ProductsModel? SelectById(ObjectId id)
        {
            return _database.SelectById(id);
        }

        public List<ProductsModel> SelectAll()
        {
            return _database.SelectAll();
        }
    }
}
