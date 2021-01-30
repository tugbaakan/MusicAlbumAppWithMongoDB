using Microsoft.EntityFrameworkCore;
using MyMusic.Core.Models;
using MyMusic.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data.Repositories
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        private DataContext DataContext
        {
            get { return _context as DataContext; }
        }
        public ArtistRepository(DataContext context): base(context)
        {
            
        }

        public async Task<IEnumerable<Artist>> GetAllWithMusicsAsync()
        {
            return await DataContext.Artists
                .Include(a => a.Musics)
                .ToListAsync();
        }

        public async Task<Artist> GetWithMusicsByIdAsync(int id)
        {
            return await DataContext.Artists
                .Include(a => a.Musics)
                .SingleOrDefaultAsync( a => a.Id == id );
        }
    }
}
