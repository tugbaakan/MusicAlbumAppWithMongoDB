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
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly IMapper _mapper;
        public ArtistController(IArtistService artistService, IMapper mapper)
        {
            _artistService = artistService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistResource>>> GetArtist()
        {
            var artists = await _artistService.GetAllWithMusics();
            if (artists == null)
                return NotFound();
            var artistsResource = _mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistResource>>(artists);
            return Ok(artistsResource);

        }

        [HttpGet("id")]
        public async Task<ActionResult<ArtistResource>> GetArtistById(int artistId)
        {
            var artist = await _artistService.GetById(artistId);

            if (artist == null)
                return NotFound();

            var artistResource = _mapper.Map<Artist, ArtistResource>(artist);

            return Ok(artistResource);

        }

        [HttpPost("")]
        public async Task<ActionResult<ArtistResource>> CreateArtist([FromBody] SaveArtistResource saveArtistResource)
        {
            var validatorArtist = new SaveArtistResourceValidator();
            var validationResult = await validatorArtist.ValidateAsync(saveArtistResource);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var artist = _mapper.Map<SaveArtistResource, Artist>(saveArtistResource);

            var newArtist = await _artistService.Create(artist);

            var newArtistResource = _mapper.Map<Artist, ArtistResource>(newArtist);

            return Ok(newArtistResource);

        }

        [HttpPut("")]
        public async Task<ActionResult<ArtistResource>> UpdateArtist(int artistId, [FromBody] SaveArtistResource saveArtistResource)
        {
            var validatorArtist = new SaveArtistResourceValidator();
            var validationResult = await validatorArtist.ValidateAsync(saveArtistResource);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var artist = _mapper.Map<SaveArtistResource, Artist>(saveArtistResource);

            var artistToBeUpdated = await _artistService.GetById(artistId);
            if (artistToBeUpdated == null)
                return NotFound();

            var artistUpdated = await _artistService.Update(artistToBeUpdated, artist);

            var artistUpdatedResource = _mapper.Map<Artist, ArtistResource>(artistUpdated);

            return Ok(artistUpdatedResource);

        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteArtist(int artistId)
        {
            var artistToBeDeleted = await _artistService.GetById(artistId);
            if (artistToBeDeleted == null)
                return NotFound();

            await _artistService.Delete(artistToBeDeleted);

            return NoContent();

        }

    }
}