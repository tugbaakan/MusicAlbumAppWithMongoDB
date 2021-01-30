using MyMusic.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core.Services
{
    public interface IMusicService
    {
        Task<IEnumerable<Music>> GetAllWithArtists();
        Task<Music> GetById(int id);
        Task<IEnumerable<Music>> GetByArtistId(int artistId);
        Task<Music> Create(Music newMusic);
        Task<Music> Update(Music musicToBeUpdated,Music music);
        Task Delete(Music music);

    }
}
