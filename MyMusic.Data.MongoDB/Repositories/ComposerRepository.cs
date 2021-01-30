using MongoDB.Bson;
using MongoDB.Driver;
using MyMusic.Core.Models;
using MyMusic.Core.Repositories;
using MyMusic.Data.MongoDB.Setting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data.MongoDB.Repositories
{
    public class ComposerRepository : IComposerRepository
    {
        private readonly IDatabaseSettings _context;

        public ComposerRepository(IDatabaseSettings context)
        {
            _context = context;
        }
        public async Task<Composer> Create(Composer composer)
        {
            await _context.Composers.InsertOneAsync(composer);
            return composer;
        }

        public async Task<bool> Delete(string id)
        {
            ObjectId idMongo = new ObjectId(id);
            FilterDefinition<Composer> filter = Builders<Composer>.Filter.Eq(m => m.Id, idMongo);

            DeleteResult deleteResult = await _context.Composers.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Composer>> GetAll()
        {
            return await _context.Composers.Find(_ => true).ToListAsync();
        }

        public async Task<Composer> GetById(string id)
        {
            ObjectId idMongo = new ObjectId(id);
            FilterDefinition<Composer> filter = Builders<Composer>.Filter.Eq(m => m.Id, idMongo);

            return await _context.Composers.Find(filter).FirstOrDefaultAsync();
        }

        public void Update(string id, Composer composer)
        {
            ObjectId idComposer = new ObjectId(id);
            FilterDefinition<Composer> filter = Builders<Composer>.Filter.Eq(m => m.Id, idComposer);
            var update = Builders<Composer>.Update.Set("FirstName", composer.FirstName).
                Set("LastName", composer.LastName);

            _context.Composers.FindOneAndUpdate(filter, update);
        }
    }
}
