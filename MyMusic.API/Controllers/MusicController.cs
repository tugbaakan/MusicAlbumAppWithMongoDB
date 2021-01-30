using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMusic.API.Resources;
using MyMusic.API.Validations;
using MyMusic.Core.Models;
using MyMusic.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;
        private readonly IArtistService _artistService;
        private readonly IMapper _mapper;
        public MusicController(IMusicService musicService, IArtistService artistService, IMapper mapper)
        {
            _musicService = musicService;
            _artistService = artistService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MusicResource>>> GetMusic()
        {
            var musics = await _musicService.GetAllWithArtists();
            if (musics == null)
                return NotFound();
            var musicsResource = _mapper.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);
            return Ok(musicsResource);

        }

        [HttpGet("id")]
        public async Task<ActionResult<MusicResource>> GetMusicById(int musicId)
        {
            var music = await _musicService.GetById(musicId);

            if (music == null)
                return NotFound();

            var musicsResource = _mapper.Map<Music, MusicResource>(music);

            return Ok(musicsResource);

        }

        [HttpPost("")]
        public async Task<ActionResult<MusicResource>> CreateMusic([FromBody] SaveMusicResource saveMusicResource)
        {
            var validatorMusic = new SaveMusicResourceValidator();
            var validationResult = await validatorMusic.ValidateAsync(saveMusicResource);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.FirstOrDefault().ErrorMessage );
            }

            var artist = await _artistService.GetById(saveMusicResource.ArtistId);
            if (artist == null)
                return BadRequest("No such an artist");

            var music = _mapper.Map<SaveMusicResource, Music>(saveMusicResource);

            var newMusic = await _musicService.Create(music);

            var newMusicResource = _mapper.Map<Music, MusicResource>(newMusic);

            return Ok(newMusicResource);

        }

        [HttpPut("")]
        public async Task<ActionResult<MusicResource>> UpdateMusic(int musicId, [FromBody] SaveMusicResource saveMusicResource)
        {
            var validatorMusic = new SaveMusicResourceValidator();
            var validationResult = await validatorMusic.ValidateAsync(saveMusicResource);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var music = _mapper.Map<SaveMusicResource, Music>(saveMusicResource);

            var musicToBeUpdated = await _musicService.GetById(musicId);
            if (musicToBeUpdated == null)
                return NotFound();

            var musicUpdated = await _musicService.Update(musicToBeUpdated, music);

            var musicUpdatedResource = _mapper.Map<Music, MusicResource>(musicUpdated);

            return Ok(musicUpdatedResource);

        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteMusic(int musicId)
        {
            var musicToBeDeleted = await _musicService.GetById(musicId);
            if (musicToBeDeleted == null)
                return NotFound();

            await _musicService.Delete(musicToBeDeleted);

            return NoContent();

        }

        [HttpGet("Artist/id")]
        public async Task<ActionResult<IEnumerable<MusicResource>>> GetAllMusicsByArtistId (int artistId)
        {
            var artist = await _artistService.GetById(artistId);
            if (artist == null)
                return NotFound("No such an artist");

            var musics = await _musicService.GetByArtistId(artistId);
            if (musics == null)
                return NotFound();
            
            var musicsResource = _mapper.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);
            return Ok(musicsResource);

        }
    }
}
