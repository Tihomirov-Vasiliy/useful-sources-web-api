using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class UsefulSourcesContextInitializer
    {
        private readonly UsefulSourcesContext _context;

        public UsefulSourcesContextInitializer(UsefulSourcesContext context)
        {
            _context = context;
        }

        //initialize database
        public void Initialize()
        {
            if (_context.Database.IsNpgsql())
                _context.Database.Migrate();
        }
    }
}
