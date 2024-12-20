﻿using FishSpotApi.Data.Context;
using FishSpotApi.Domain.Entity;
using MongoDB.Driver;

namespace FishSpotApi.Core.Repository;

public class TokenRepository(FishSpotApiContext mongo) : BaseRepository<TokenEntity>(mongo, "token")
{
    public TokenEntity GetByActor(string actorId)
    {
        var filter = Builders<TokenEntity>.Filter.Eq(entity => entity.Actor, actorId);
        var token = _db.Find(filter).FirstOrDefault();
        return token;
    }

    public bool IsValidFields(string refreshToken, string token)
    {
        var filter = Builders<TokenEntity>.Filter.And(
            Builders<TokenEntity>.Filter.Eq(entity => entity.Token, token),
            Builders<TokenEntity>.Filter.Eq(entity => entity.RefreshToken, refreshToken));
        var registry = _db.Find(filter).FirstOrDefault();

        return registry != null;
    }
}