using FishSpotApi.Data.Context;
using FishSpotApi.Domain.Entity;
using MongoDB.Driver;

namespace FishSpotApi.Core.Repository;

public class UserRepository(FishSpotApiContext mongo) : BaseRepository<UserEntity>(mongo, "user")
{
    public List<UserEntity> GetByEmail(string email)
    {
        var filter = Builders<UserEntity>.Filter.Eq(entity => entity.Email, email);
        var token = _db.Find(filter).ToList();
        return token;
    }
}