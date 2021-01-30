using MyMusic.Core.Models;
using MyMusic.Core.Repositories;
using MyMusic.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Services.Services
{
    public class ComposerService : IComposerService
    {
        private readonly IComposerRepository _composerRepository;
        public ComposerService(IComposerRepository ComposerRepository)
        {
            _composerRepository = ComposerRepository;
        }

        public async Task<Composer> Create(Composer composer)
        {
            return await _composerRepository.Create(composer);
        }

        public async Task<bool> Delete(string id)
        {
            return await _composerRepository.Delete(id);
        }

        public async Task<IEnumerable<Composer>> GetAll()
        {
            return await _composerRepository.GetAll();
        }

        public async Task<Composer> GetById(string id)
        {
            return await _composerRepository.GetById(id);
        }

        public void Update(string id, Composer composer)
        {
            _composerRepository.Update(id, composer);
        }
    }
}
