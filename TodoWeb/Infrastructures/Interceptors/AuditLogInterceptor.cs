using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures.Interceptors;

public class AuditLogInterceptor : SaveChangesInterceptor
{
    /*
    private HashSet<object> addSet = new HashSet<object>();
    */
    
    private List<EntityEntry> addEntities = new List<EntityEntry>();
    
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var context = eventData.Context as ApplicationDbContext;
        
        var auditLogs = new List<AuditLog>();
        
        foreach (var entry in context.ChangeTracker.Entries()) 
        {
            if (entry.Entity is AuditLog)
            {
                continue;
            }
            var log = new AuditLog
            {
                EntityName = entry.Entity.GetType().Name,
                CreateAt = DateTime.Now,
                Action = entry.State.ToString(),
                
            };
            if (entry.State == EntityState.Added)
            {
                addEntities.Add(entry);
                //log.NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject());
            }
            if (entry.State == EntityState.Modified)
            {
                log.OldValue = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                log.NewValue = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                auditLogs.Add(log);

            }
            if (entry.State == EntityState.Deleted)
            {
                log.OldValue = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                auditLogs.Add(log);

            }
            /*
            auditLogs.Add(log);
            */
            if (auditLogs.Any())
            {
                context.AuditLog.AddRange(auditLogs); //State = added
            }
        }
        return base.SavingChanges(eventData, result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var context = eventData.Context as ApplicationDbContext;
        
        
        if (addEntities.Any())
        {
            var auditLogs = addEntities.Select(entity => new AuditLog
            {
                EntityName = entity.Entity.GetType().Name, //student
                CreateAt = DateTime.Now,
                Action = EntityState.Added.ToString(),
                NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject())
            });
                context.AuditLog.AddRange(auditLogs);
                
                addEntities.Clear();
                
                context.SaveChanges();
            }
            
        
        return base.SavedChanges(eventData, result);
    }
}