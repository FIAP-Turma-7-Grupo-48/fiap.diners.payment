using MongoDB.Driver;

namespace Infrastructure.Context.Interfaces;

public interface IMongoContext
{
    IMongoCollection<T> GetCollection<T>(); 

}
