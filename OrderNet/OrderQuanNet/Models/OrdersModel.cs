using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OrderQuanNet.Services;

namespace OrderQuanNet.Models
{
    public class OrdersModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        public ObjectId? _id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? users_id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? product_id { get; set; }

        internal object date;
        public int? amount { get; set; }
        public int? price { get; set; }
        public string? status { get; set; }
        public string? created_at { get; set; }
        public string? updated_at { get; set; }

        public void create()
        {
            OrdersService ordersService = new OrdersService();
            this.created_at = DateTime.UtcNow.ToString("o");
            this.updated_at = DateTime.UtcNow.ToString("o");

            ordersService.Insert(this);
        }

        public void save()
        {
            OrdersService ordersService = new OrdersService();
            this.updated_at = DateTime.UtcNow.ToString("o");
            ordersService.Update(this);
        }

        public void delete()
        {
            OrdersService ordersService = new OrdersService();
            ordersService.Delete(this);
        }
    }
}
