﻿using FishspotApi.Data.Context;
using FishspotApi.Domain.Entity;
using MongoDB.Driver;

namespace FishspotApi.Core.Repository
{
    public abstract class BaseRepository<T> where T : BaseEntity
    {
        public readonly IMongoCollection<T>? _db;

        public BaseRepository(FishspotApiContext mongo, string collection)
        {
            _db = mongo.Database?.GetCollection<T>(collection);
        }

        public IEnumerable<T> Get()
        {
            return _db.Find(FilterDefinition<T>.Empty).ToEnumerable();
        }

        public T Get(string id)
        {
            var filter = Builders<T>.Filter.Eq(entity => entity.Id, id);
            var genericEntity = _db.Find(filter).FirstOrDefault();
            return genericEntity;
        }

        public T Insert(T genericEntity)
        {
            _db?.InsertOne(genericEntity);
            return genericEntity;
        }

        public T Update(T token)
        {
            var filter = Builders<T>.Filter.Eq(entity => entity.Id, token.Id);
            _db?.ReplaceOne(filter, token);
            return token;
        }

        public void Delete(string id)
        {
            _db?.DeleteOne(id);
        }
    }
}