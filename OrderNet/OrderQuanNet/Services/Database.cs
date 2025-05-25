using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace OrderQuanNet.Services
{
    internal class Database<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;

        public Database(string collectionName)
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("OrderQuanNet");
            _collection = _database.GetCollection<T>(collectionName);
        }

        public async void Insert(T item)
        {
            if (item == null) return;
            await _collection.InsertOneAsync(item);
        }

        // Update theo _id trong item
        public bool Update(T item)
        {
            if (item == null) return false;

            var idValue = GetIdValue(item);
            if (idValue == null) return false;

            var filter = Builders<T>.Filter.Eq("_id", idValue);
            var result = _collection.ReplaceOne(filter, item);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        // Delete theo _id
        public bool Delete(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            var result = _collection.DeleteOne(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        // Overload Delete nhận T (lấy id rồi gọi Delete id)
        public bool Delete(T item)
        {
            var idValue = GetIdValue(item);
            if (idValue == null) return false;
            return Delete((ObjectId)idValue);
        }

        public List<T> Select(T filterObject, bool includeNulls = false)
        {
            var filter = CreateFilter(filterObject, includeNulls);
            return _collection.Find(filter).ToList();
        }

        public T? SelectById(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return _collection.Find(filter).FirstOrDefault();
        }

        public List<T> SelectAll(T? filterObject = null, bool includeNulls = false)
        {
            if (filterObject == null)
                return _collection.Find(Builders<T>.Filter.Empty).ToList();

            return Select(filterObject, includeNulls);
        }


        private FilterDefinition<T> CreateFilter(T item, bool includeNulls)
        {
            var filters = new List<FilterDefinition<T>>();
            var builder = Builders<T>.Filter;

            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(item);

                if (value != null || includeNulls)
                {
                    string fieldName = prop.Name;

                    if (string.Equals(fieldName, "id", StringComparison.OrdinalIgnoreCase))
                        fieldName = "_id";

                    filters.Add(builder.Eq(fieldName, value ?? BsonNull.Value));
                }
            }

            return filters.Count > 0 ? builder.And(filters) : builder.Empty;
        }

        private object? GetIdValue(T item)
        {
            var idProp = typeof(T).GetProperties().FirstOrDefault(p =>
                string.Equals(p.Name, "id", StringComparison.OrdinalIgnoreCase) ||
                p.GetCustomAttributes(typeof(BsonIdAttribute), false).Any());

            if (idProp == null)
                return null;

            return idProp.GetValue(item);
        }

        public List<BsonDocument> AggregateWithLookup(string fromCollection, string localField, string foreignField, string asField)
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", fromCollection },
                    { "localField", localField },
                    { "foreignField", foreignField },
                    { "as", asField }
                })
            };
            return _collection.Aggregate<BsonDocument>(pipeline).ToList();
        }

        public List<BsonDocument> TextSearch(string searchField, string searchText)
        {
            var collectionName = _collection.CollectionNamespace.CollectionName;
            var collection = _database.GetCollection<BsonDocument>(collectionName);

            var filter = Builders<BsonDocument>.Filter.Text(searchText);
            return collection.Find(filter).ToList();
        }

        public List<BsonDocument> GroupAndSum(string groupByField, string sumField)
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$" + groupByField },
                    { "total", new BsonDocument("$sum", "$" + sumField) }
                })
            };
            return _collection.Aggregate<BsonDocument>(pipeline).ToList();
        }

        public List<BsonDocument> UnwindField(string arrayField)
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$unwind", "$" + arrayField)
            };
            return _collection.Aggregate<BsonDocument>(pipeline).ToList();
        }

        public List<BsonDocument> FilterByDateRange(string dateField, DateTime startDate, DateTime endDate)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Gte(dateField, startDate) & builder.Lte(dateField, endDate);
            var collectionName = _collection.CollectionNamespace.CollectionName;
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            return collection.Find(filter).ToList();
        }
    }
}
