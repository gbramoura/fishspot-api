using FishspotApi.Data.Context;
using FishspotApi.Domain.Entity;
using MongoDB.Driver;

namespace FishspotApi.Core.Repository
{
    public class UserRepository(FishspotApiContext mongo) : BaseRepository<UserEntity>(mongo, "user")
    {
        public IEnumerable<UserEntity> GetByEmail(string email)
        {
            var filter = Builders<UserEntity>.Filter.Eq(entity => entity.Email, email);
            var token = _db.Find(filter).ToEnumerable();
            return token;
        }
    }
}