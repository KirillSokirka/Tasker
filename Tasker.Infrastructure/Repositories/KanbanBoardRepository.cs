﻿using Microsoft.EntityFrameworkCore;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Infrastructure.Repositories;

public class KanbanBoardRepository : EntityRepository<KanbanBoard>
{
    public KanbanBoardRepository(ApplicationContext context) : base(context)
    {
    }

    public override async Task<KanbanBoard?> GetByIdAsync(string id)
        => await DbSet
            .AsNoTracking()
            .Include(t => t.Project).ThenInclude(c => c.AssignedProjectUsers)
            .Include(t => t.Project).ThenInclude(c => c.AdminProjectUsers)
            .Include(t => t.Columns).ThenInclude(c => c.Tasks).ThenInclude(c => c.Assignee)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

    public override async Task<List<KanbanBoard>> GetAllAsync()
        => await DbSet
            .AsNoTracking()
            .Include(t => t.Project)
            .Include(t => t.Columns).ThenInclude(c => c.Tasks)
            .AsNoTracking()
            .ToListAsync();
}