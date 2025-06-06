﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OrderQuanNet.Services;

namespace OrderQuanNet.Models
{
    public class ProductsModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public ObjectId? _id { get; set; }
        public string? name { get; set; }
        public string? type { get; set; }
        public int? price { get; set; }
        public string? image_path { get; set; }

        public void create()
        {
            ProductsService productsService = new ProductsService();
            productsService.Insert(this);
        }

        public void save()
        {
            ProductsService productsService = new ProductsService();
            productsService.Update(this);
        }

        public void delete()
        {
            ProductsService productsService = new ProductsService();
            productsService.Delete(this);
        }
    }
}
