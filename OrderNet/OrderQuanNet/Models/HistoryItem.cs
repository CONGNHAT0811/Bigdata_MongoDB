using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderQuanNet.Models
{
    public class HistoryItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public int id { get; set; }
        public string name { get; set; }
        public string image_path { get; set; }
        public decimal price { get; set; }
        public string status { get; set; }
        public int amount { get; set; }
    }
}
