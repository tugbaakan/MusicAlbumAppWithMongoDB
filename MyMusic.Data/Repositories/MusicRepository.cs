using Microsoft.EntityFrameworkCore;
using MyMusic.Core.Models;
using MyMusic.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data.Repositories
{
    public class MusicRepository : Repository<Music>, IMusicRepository
    {
        private DataContext DataContext
        {
            get { return _context as DataContext; }
        }
        public MusicRepository(DataContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Music>> GetAllWithArtistsAsync()
        {
            return await DataContext.Musics
                .Include(m => m.Artist)
                .ToListAsync();
        }

        public async Task<Music> GetWithArtistByIdAsync(int id)
        {
            return await DataContext.Musics
              .Include(m => m.Artist)
              .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Music>> GetAllWithArtistsByArtistIdAsync(int artistId)
        {
            return await DataContext.Musics
                 .Include(m => m.Artist)
                 .Where(m => m.ArtistId == artistId)
                 .ToListAsync();
        }
    }
}
