using System.Net;
using Infrastructure.Data;
using Domain.Entities;
using Domain.Exceptions;

namespace Services.ApplicationServices
{
    public class TagsService : ITagsService
    {
        private IUsefulSourcesRepo _repository;
        public TagsService(IUsefulSourcesRepo repository)
        {
            _repository = repository;
        }
        public IEnumerable<Tag> GetTags(string name=null, int parentTagId=0, string parentTagName= null)
        {
            IEnumerable<Tag> tags = _repository.GetAllTags();

            //Возможно перенести данную проверку в запрос к бд?
            if (name!=null && tags.Count() != 0)
                tags = tags.Where(t=>t.Name==name);
            if (parentTagId != 0 && tags.Count() != 0)
                tags = tags.Where(t => t.ParentTags.Any(t=>t.Id==parentTagId));
            if (parentTagName != null && tags.Count() != 0)
                tags = tags.Where(t => t.ParentTags.Any(t => t.Name == parentTagName));

            if (tags != null && tags.Count()!=0)
            {
                return tags;
            }
            throw new ObjectNotFoundException(ErrorMessages.TAGS_NOT_FOUNDED);
        }
        public Tag GetTagById(int id)
        {
            Tag tag = _repository.GetTagById(id);

            if (tag != null)
            {
                return tag;
            }
            throw new ObjectNotFoundException(ErrorMessages.TAG_NOT_FOUNDED + id);
        }
        public bool CreateTag(Tag tag)
        {
            if (tag != null)
            {
                List<int> tagIds = new List<int>();
                if (tag.ParentTags != null)
                    foreach (var parentTag in tag.ParentTags)
                        tagIds.Add(parentTag.Id);

                var tagsFromDataBase = GetTags().Where(tag => tagIds.Contains(tag.Id)).ToList();

                if (tagsFromDataBase.Count == tagIds.Count)
                {
                    tag.ParentTags = tagsFromDataBase;
                    return _repository.CreateTag(tag);
                }
                else
                    throw new WrongInputDataException(ErrorMessages.CREATE_TAG_WRONG_INPUT, HttpStatusCode.BadRequest);
            }
            throw new ObjectNotFoundException(ErrorMessages.CREATE_TAG_NULL_TAG_INPUT);
        }
        public bool UpdateTag(Tag tagModel, Tag patchTag)
        {
            var parentTagsFromDb = new List<Tag>();
            Tag currentTagFromDb;

            if (patchTag.ParentTags == null)
            {
                parentTagsFromDb = null;
                tagModel.ParentTags = null;
            }
            else
            {
                foreach (var parentTag in patchTag.ParentTags)
                {
                    currentTagFromDb = _repository.GetTagById(parentTag.Id);
                    if (currentTagFromDb == null)
                        throw new WrongInputDataException(ErrorMessages.UPDATE_TAG_WRONG_TAG_INPUT + parentTag.Id, HttpStatusCode.BadRequest);
                    if (currentTagFromDb == tagModel)
                        throw new WrongInputDataException(ErrorMessages.UPDATE_TAG_SAME_TAG_INPUT, HttpStatusCode.BadRequest);
                    parentTagsFromDb.Add(currentTagFromDb);
                }
                tagModel.ParentTags = parentTagsFromDb.OrderBy(t => t.Id);
            }
            tagModel.Name = patchTag.Name;
            return _repository.UpdateTag(tagModel);
        }
        public bool DeleteTag(int id)
        {
            Tag tag = GetTagById(id);
            bool canDelete = true;

            if (tag == null)
                throw new ObjectNotFoundException(ErrorMessages.TAG_NOT_FOUNDED);

            if (tag.TagsOf != null && tag.TagsOf.Count() > 0)
                canDelete = false;

            if (canDelete)
                return _repository.DeleteTag(tag);

            string returnIds = string.Empty;

            foreach (var item in tag.TagsOf)
            {
                returnIds += " " + item.Id.ToString();
            }
            throw new WrongInputDataException(ErrorMessages.DELETE_TAG_WRONG_INPUT + returnIds, HttpStatusCode.BadRequest);
        }
    }
}
