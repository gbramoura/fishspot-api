using FishSpotApi.Data.Context;
using FishSpotApi.Domain.Entity;
using MongoDB.Driver;

namespace FishSpotApi.Core.Repository;

public class UserRepository(FishSpotApiContext mongo) : BaseRepository<UserEntity>(mongo, "user")
{
    public List<UserEntity> GetByEmail(string email)
    {
        var filter = Builders<UserEntity>.Filter.Eq(entity => entity.Email, email);
        var list = _db.Find(filter).ToList();
        return list;
    }
    
    public List<UserEntity> GetByUsername(string username)
    {
        var filter = Builders<UserEntity>.Filter.Eq(entity => entity.Username, username);
        var list = _db.Find(filter).ToList();
        return list;
    }
}