using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.ApplicationServices;
using WebApi.Dtos;
using Domain.Entities;

namespace WebApi.Controllers
{
    [Route("api/sources")]
    [ApiController]
    public class SourcesController : ControllerBase
    {
        private readonly ISourcesService _sourceService;
        private readonly IMapper _mapper;

        public SourcesController(ISourcesService appService, IMapper mapper)
        {
            _sourceService = appService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all sources with tags
        /// </summary>
        /// <response code="200">Returns all sources from database</response>
        /// <response code="404">Returns an error when there are no sources in database</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<SourceReadDto>> GetAllSources(string name, string uri, int tagId, string tagName)
        {
            return Ok(_mapper.Map<IEnumerable<SourceReadDto>>(_sourceService.GetSources(name, uri, tagId, tagName)));
        }

        /// <summary>
        /// Get source by id with tags
        /// </summary>
        /// <response code="200">Returns source by id from database</response>
        /// <response code="404">Returns an error when there is no source with that id in database</response>
        [HttpGet("{id}", Name = "GetSourceById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public ActionResult<SourceReadDto> GetSourceById(int id)
        {
            return Ok(_mapper.Map<SourceReadDto>(_sourceService.GetSourceById(id)));
        }

        /// <summary>
        /// Create single source
        /// </summary>
        /// <response code="201">Creates single source</response>
        /// <response code="400">Returns an error when you try to create source in the wrong way</response>
        /// <response code="401">Returns an error when you are not authorized</response>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<SourceReadDto> CreateSource(SourceCreateDto sourceCreateDto)
        {
            Source sourceModel = _mapper.Map<Source>(sourceCreateDto);
            _sourceService.CreateSource(sourceModel);
            SourceReadDto sourceReadDto = _mapper.Map<SourceReadDto>(sourceModel);

            return CreatedAtRoute(nameof(GetSourceById), new { Id = sourceReadDto.Id }, sourceReadDto);
        }

        /// <summary>
        /// Delete source by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">Returns when you deleted source</response>
        /// <response code="401">Returns an error when you are not authorized</response>
        /// <response code="404">Returns an error when there is no source with id</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status404NotFound)]
        public ActionResult DeleteSource(int id)
        {
            _sourceService.DeleteSource(id);
            return NoContent();
        }

        /// <summary>
        /// Partially updates source
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        /// <response code="200">Returns updated model</response>  
        /// <response code="400">Returns an error when you try to update source with wrong input data</response>
        /// <response code="401">Returns an error when you are not authorized</response>
        /// <response code="404">Returns an error when there is no source with id</response>
        [HttpPatch("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(SourceReadDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public ActionResult PartialSourceUpdate(int id, JsonPatchDocument<SourceUpdateDto> patchDoc)
        {
            var sourceModel = _sourceService.GetSourceById(id);
            var sourceToPatch = _mapper.Map<SourceUpdateDto>(sourceModel);
            patchDoc.ApplyTo(sourceToPatch, ModelState);

            if (!TryValidateModel(sourceToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _sourceService.UpdateSource(sourceModel, _mapper.Map<Source>(sourceToPatch));

            return Ok(_mapper.Map<SourceReadDto>(sourceModel));
        }
    }
}
