using System.Net;
using Infrastructure.Data;
using Domain.Entities;
using Domain.Exceptions;

namespace Services.ApplicationServices
{
    public class SourcesService : ISourcesService
    {
        private IUsefulSourcesRepo _repository;
        public SourcesService(IUsefulSourcesRepo repository)
        {
            _repository = repository;
        }
        public IEnumerable<Source> GetSources(string name = null, string uri = null, int tagId = 0, string tagName = null)
        {
            IEnumerable<Source> sources = _repository.GetAllSourcesWithTreeTags();

            //Возможно перенести данную проверку в запрос к бд?
            if (name != null && sources.Count() != 0)
                sources = sources.Where(s => s.Name == name);
            if (uri != null && sources.Count() != 0)
                sources = sources.Where(s => s.Uri == uri);
            if (tagId != 0 && sources.Count() != 0)
                sources = sources.Where(s => s.Tags.Any(t => t.Id == tagId));
            if (tagName != null && sources.Count() != 0)
                sources = sources.Where(s => s.Tags.Any(t => t.Name == tagName));

            if (sources != null && sources.Count() != 0)
            {
                return sources;
            }
            throw new ObjectNotFoundException(ErrorMessages.GET_SOURCES_404_NOT_FOUNDED);
        }
        public Source GetSourceById(int id)
        {
            Source source = _repository.GetSourceWithTagsById(id);
            if (source != null)
            {
                return source;
            }
            throw new ObjectNotFoundException(ErrorMessages.SOURCE_NOT_FOUNDED + id);
        }
        public bool CreateSource(Source source)
        {
            if (source != null)
            {
                List<int> tagIds = new List<int>();
                if (source.Tags != null)
                    foreach (var tag in source.Tags)
                        tagIds.Add(tag.Id);
                else
                    return _repository.CreateSource(source);

                var tagsFromDataBase = _repository.GetAllTags().Where(tag => tagIds.Contains(tag.Id)).ToList();

                if (tagsFromDataBase.Count == tagIds.Count)
                {
                    source.Tags = tagsFromDataBase;
                    return _repository.CreateSource(source);
                }
                else
                    throw new WrongInputDataException(ErrorMessages.CREATE_SOURCE_WRONG_TAG_INPUT, HttpStatusCode.BadRequest);
            }
            throw new ObjectNotFoundException(ErrorMessages.CREATE_SOUCE_NULL_SOURCE_INPUT);
        }
        public bool UpdateSource(Source sourceModel, Source patchSource)
        {
            var tagsFromDb = new List<Tag>();
            Tag currentTagFromDb;

            if (patchSource.Tags == null)
                tagsFromDb = null;
            else
                foreach (var tagOfSource in patchSource.Tags)
                {
                    currentTagFromDb = _repository.GetTagById(tagOfSource.Id);
                    if (currentTagFromDb == null)
                        throw new WrongInputDataException(ErrorMessages.UPDATE_SOURCE_WRONG_TAG_INPUT + tagOfSource.Id, HttpStatusCode.BadRequest);
                    tagsFromDb.Add(currentTagFromDb);
                }

            sourceModel.Name = patchSource.Name;
            sourceModel.Uri = patchSource.Uri;
            sourceModel.Tags = tagsFromDb;

            return _repository.UpdateSource(sourceModel);
        }
        public bool DeleteSource(int id)
        {
            Source source = GetSourceById(id);
            if (source == null)
            {
                throw new ObjectNotFoundException(ErrorMessages.SOURCE_NOT_FOUNDED);
            }
            return _repository.DeleteSource(source);
        }
    }
}
