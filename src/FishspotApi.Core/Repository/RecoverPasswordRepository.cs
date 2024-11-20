using FishspotApi.Data.Context;
using FishspotApi.Domain.Entity;
using MongoDB.Driver;

namespace FishspotApi.Core.Repository
{
    public class RecoverPasswordRepository(FishspotApiContext mongo) : BaseRepository<RecoverPasswordEntity>(mongo, "recover_password")
    {

        public RecoverPasswordEntity GetByTokenAndEmail(string token, string email)
        {
            var filter = Builders<RecoverPasswordEntity>.Filter.And(
                Builders<RecoverPasswordEntity>.Filter.Eq(entity => entity.Token, token),
                Builders<RecoverPasswordEntity>.Filter.Eq(entity => entity.Email, email));

            return _db.Find(filter).FirstOrDefault();
        }
    }
}
