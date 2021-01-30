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
    public class ComposerController : ControllerBase
    {
        private readonly IComposerService _composerService;
        private readonly IMapper _mapper;
        public ComposerController(IComposerService composerService, IMapper mapper)
        {
            _composerService = composerService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ComposerResource>>> GetAllComposers()
        {
            var composers = await _composerService.GetAll();
            if (composers == null)
                return NotFound();
            var composerResources = _mapper.Map<IEnumerable<Composer>, IEnumerable<ComposerResource>>(composers);
            return Ok(composerResources);

        }

        [HttpGet("id")]
        public async Task<ActionResult<IEnumerable<ComposerResource>>> GetComposerById(string composerId)
        {
            var composer = await _composerService.GetById(composerId);
            
            if (composer == null)
                return NotFound();

            var composerResource = _mapper.Map<Composer, ComposerResource>(composer);
            return Ok(composerResource);

        }

        [HttpPost("")]
        public async Task<ActionResult<ComposerResource>> CreateComposer([FromBody] SaveComposerResource saveComposerResource)
        {
            var validatorComposer = new SaveComposerResourceValidator();
            var validationResult = await validatorComposer.ValidateAsync(saveComposerResource);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var composerNew = _mapper.Map<SaveComposerResource, Composer>(saveComposerResource);

            var composerCreated = await _composerService.Create(composerNew);
            var composerCreatedResource = _mapper.Map<Composer, ComposerResource>(composerCreated);
            return Ok(composerCreatedResource);


        }
    }
}
