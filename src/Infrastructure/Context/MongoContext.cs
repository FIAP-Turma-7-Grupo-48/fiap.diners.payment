using Infrastructure.Context.Interfaces;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Context;

[ExcludeFromCodeCoverage]
public class MongoContext : IMongoContext
{
    private readonly string _connectionString;
    private readonly string _databaseName;

    public MongoContext(string connectionString, string databaseName)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
    }

    public MongoUrl MongoUrl => new MongoUrl(_connectionString);
    public MongoClient Client => new MongoClient(MongoUrl);
    public IMongoDatabase Database => Client.GetDatabase(_databaseName);
    public string GetCollectionName<T>()
    {
        var customAttribute = Attribute.GetCustomAttribute(typeof(T), typeof(BsonDiscriminatorAttribute));
        if (customAttribute != null)
        {
            var bsonClassMap = BsonClassMap.LookupClassMap(typeof(T));
            if (string.IsNullOrEmpty(bsonClassMap.Discriminator) is false)
            {
                return bsonClassMap.Discriminator;

            }
        }
        var name = typeof(T).Name;
        return
            name;
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return
            Database.GetCollection<T>(collectionName);
    }

    public IMongoCollection<T> GetCollection<T>()
    {
        var collectionName = GetCollectionName<T>();
        return
            Database.GetCollection<T>(collectionName);
    }
}
