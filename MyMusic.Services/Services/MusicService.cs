using MyMusic.Core;
using MyMusic.Core.Models;
using MyMusic.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Services.Services
{
    public class MusicService : IMusicService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MusicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Music> Create(Music newMusic)
        {
            await _unitOfWork.MusicRepository.AddAsync(newMusic);
            await _unitOfWork.CommitAsync();
            return newMusic;
        }

        public async Task Delete(Music music)
        {
            _unitOfWork.MusicRepository.Remove(music);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Music>> GetAllWithArtists()
        {
            return await _unitOfWork.MusicRepository.GetAllWithArtistsAsync();
        }

        public async Task<IEnumerable<Music>> GetByArtistId(int artistId)
        {
            return await _unitOfWork.MusicRepository.GetAllWithArtistsByArtistIdAsync(artistId);
        }

        public async Task<Music> GetById(int id)
        {
            return await _unitOfWork.MusicRepository.GetByIdAsync(id);
        }

        public async Task<Music> Update(Music musicToBeUpdated, Music music)
        {
            musicToBeUpdated.Name = music.Name;
            musicToBeUpdated.ArtistId = music.ArtistId;

            await _unitOfWork.CommitAsync();
            return await _unitOfWork.MusicRepository.GetByIdAsync(musicToBeUpdated.Id);
        }
    }
}
