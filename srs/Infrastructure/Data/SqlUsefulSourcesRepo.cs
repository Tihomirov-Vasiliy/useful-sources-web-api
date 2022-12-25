using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Data
{
    public class SqlUsefulSourcesRepo : IUsefulSourcesRepo
    {
        private readonly UsefulSourcesContext _context;
        public SqlUsefulSourcesRepo(UsefulSourcesContext context)
        {
            _context = context;
        }
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
        public IEnumerable<Source> GetAllSourcesWithTreeTags()
        {
            ICollection<Source> sources = _context.Sources
                .Include(s => s.Tags)
                .ThenInclude(t => t.ParentTags)
                .AsNoTracking()
                .ToList();

            return sources;
        }
        public Source GetSourceWithTagsById(int id)
        {
            Source source = _context.Sources
                .Include(s => s.Tags)
                .ThenInclude(t => t.ParentTags)
                .FirstOrDefault(s => s.Id == id);

            return source;
        }
        public bool CreateSource(Source source)
        {
            _context.Sources.Add(source);
            return SaveChanges();
        }
        public bool UpdateSource(Source source)
        {
            return SaveChanges();
            //ef core is tracking entities therefore we don't need to update entity here
        }
        public bool DeleteSource(Source source)
        {
            _context.Sources.Remove(source);
            return SaveChanges();
        }
        public IEnumerable<Tag> GetAllTags()
        {
            var tags = _context.Tags
                .Include(t => t.ParentTags)
                .AsNoTracking()
                .ToList();

            return tags;
        }
        public Tag GetTagById(int id)
        {
            var tag = _context.Tags
                .Include(t => t.ParentTags)
                .Include(t => t.TagsOf)
                .FirstOrDefault(t => t.Id == id);

            return tag;
        }
        public bool CreateTag(Tag tag)
        {
            _context.Tags.Add(tag);
            return SaveChanges();
        }
        public bool UpdateTag(Tag tag)
        {
            return SaveChanges();
            //ef core is tracking entities therefore we don't need to update entity here
        }
        public bool DeleteTag(Tag tag)
        {
            _context.Tags.Remove(tag);
            return SaveChanges();
        }
        public string GetAuthenticatedRole(string userLogin, string userPassword)
        {
            var user = _context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Login == userLogin && u.Password == userPassword);

            if (user != null)
                return user.Role;
            return null;
        }
    }
}