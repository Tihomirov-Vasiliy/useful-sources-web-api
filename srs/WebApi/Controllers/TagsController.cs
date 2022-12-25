using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.ApplicationServices;
using WebApi.Dtos;
using Domain.Entities;

namespace WebApi.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _tagService;
        private readonly IMapper _mapper;

        public TagsController(ITagsService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all tags with parent tags (first level of tag tree)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentTagId"></param>
        /// <param name="parentTagName"></param>
        /// <returns></returns>
        /// <response code="200">Returns all tags</response>
        /// <response code="404">Returns error when there are no tags in database</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TagReadWithParentPropDto>> GetAllTags(string name, int parentTagId, string parentTagName)
        {
            return Ok(_mapper.Map<IEnumerable<TagReadWithParentPropDto>>(_tagService.GetTags(name, parentTagId, parentTagName)));
        }

        /// <summary>
        /// Get tag by id with parent tags (first level of tag tree)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns tag by id</response>
        /// <response code="404">Returns when there is no tag with id</response>
        [HttpGet("{id}", Name = "GetTagById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TagReadWithParentPropDto>> GetTagById(int id)
        {
            return Ok(_mapper.Map<TagReadWithParentPropDto>(_tagService.GetTagById(id)));
        }

        /// <summary>
        /// Create single tag
        /// </summary>
        /// <param name="tagCreateDto"></param>
        /// <returns></returns>
        /// <response code="201">Returns created tag</response>
        /// <response code="400">>Returns an error when you try to create tag in the wrong way</response>
        /// <response code="401">Returns an error when you are not authorized</response>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(TagReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<TagReadDto> CreateTag(TagCreateDto tagCreateDto)
        {
            Tag tagModel = _mapper.Map<Tag>(tagCreateDto);
            _tagService.CreateTag(tagModel);
            TagReadWithParentPropDto tagReadDto = _mapper.Map<TagReadWithParentPropDto>(tagModel);

            return CreatedAtRoute(nameof(GetTagById), new { Id = tagReadDto.Id }, tagReadDto);
        }

        /// <summary>
        /// Delete single tag
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">Returns when you deleted tag</response>
        /// <response code="400">Returns an error when you try to delete tag with children tags</response>
        /// <response code="401">Returns an error when you are not authorized</response>
        /// <response code="404">Returns an error when there is no tag with id</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public ActionResult DeleteTag(int id)
        {
            _tagService.DeleteTag(id);

            return NoContent();
        }
        /// <summary>
        /// Partially updates tag
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        /// <response code="200">Returns updated tag</response>
        /// <response code="400">Returns an error when you try to update tag with wrong input data</response>
        /// <response code="401">Returns an error when you are not authorized</response>
        /// <response code="404">Returns an error when there is no tag with id</response>
        [HttpPatch("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(TagReadWithParentPropDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public ActionResult<TagReadWithParentPropDto> PartialTagUpdate(int id, JsonPatchDocument<TagUpdateDto> patchDoc)
        {

            //возможно не будет отслеживаться сущность!
            Tag tagModel = _tagService.GetTagById(id);
            TagUpdateDto tagToPatch = _mapper.Map<TagUpdateDto>(tagModel);
            patchDoc.ApplyTo(tagToPatch, ModelState);
            if (!TryValidateModel(tagToPatch))
                return ValidationProblem(ModelState);

            _tagService.UpdateTag(tagModel, _mapper.Map<Tag>(tagToPatch));

            return Ok(_mapper.Map<TagReadWithParentPropDto>(tagModel));
        }
    }
}
