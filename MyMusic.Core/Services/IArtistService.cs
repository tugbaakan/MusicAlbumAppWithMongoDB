using MyMusic.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core.Services
{
    public interface IArtistService
    {
        Task<IEnumerable<Artist>> GetAllWithMusics();
        Task<Artist> GetById(int id);
        Task<Artist> Create(Artist newArtist);
        Task<Artist> Update(Artist artistToBeUpdated, Artist artist);
        Task Delete(Artist artist);
    }
}
