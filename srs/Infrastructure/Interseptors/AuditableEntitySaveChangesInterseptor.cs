using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Application.Common.Interfaces;
using Domain.Common;

namespace Infrastructure.Interseptors
{
    public class AuditableEntitySaveChangesInterseptor : SaveChangesInterceptor
    {
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService _userService;

        public AuditableEntitySaveChangesInterseptor(IDateTime dateTime, ICurrentUserService userService)
        {
            _dateTime = dateTime;
            _userService = userService;
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public void UpdateEntities(DbContext context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = _dateTime.Now;
                    entry.Entity.CreatedBy = _userService.UserId;
                    entry.Entity.UpdatedAt = _dateTime.Now;
                    entry.Entity.UpdatedBy = _userService.UserId;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = _dateTime.Now;
                    entry.Entity.UpdatedBy = _userService.UserId;
                }
            }
        }
    }
}
