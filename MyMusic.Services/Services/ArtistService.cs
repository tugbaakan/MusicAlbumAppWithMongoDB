using MyMusic.Core;
using MyMusic.Core.Models;
using MyMusic.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Services.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ArtistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Artist> Create(Artist newArtist)
        {
            await _unitOfWork.ArtistRepository.AddAsync(newArtist);
            await _unitOfWork.CommitAsync();

            return newArtist;
        }

        public async Task Delete(Artist artist)
        {
            _unitOfWork.ArtistRepository.Remove(artist);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Artist>> GetAllWithMusics()
        {
            return await _unitOfWork.ArtistRepository.GetAllWithMusicsAsync();
        }

        public async Task<Artist> GetById(int id)
        {
            return await _unitOfWork.ArtistRepository.GetByIdAsync(id);
        }

        public async Task<Artist> Update(Artist artistToBeUpdated, Artist artist)
        {
            artistToBeUpdated.Name = artist.Name;

            await _unitOfWork.CommitAsync();

            return await _unitOfWork.ArtistRepository.GetByIdAsync(artistToBeUpdated.Id);
        }
    }
}
