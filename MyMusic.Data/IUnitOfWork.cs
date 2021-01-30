using MyMusic.Core;
using MyMusic.Core.Repositories;
using MyMusic.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private IMusicRepository _musicRepository;
        private IArtistRepository _artistRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }
        public IMusicRepository MusicRepository => _musicRepository = _musicRepository ?? new MusicRepository(_context);

        public IArtistRepository ArtistRepository => _artistRepository = _artistRepository ?? new ArtistRepository(_context);
        public IUserRepository UserRepository => _userRepository = _userRepository ?? new UserRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
