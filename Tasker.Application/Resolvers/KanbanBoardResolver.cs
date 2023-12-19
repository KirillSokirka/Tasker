using Microsoft.EntityFrameworkCore;
using Tasker.Application.Interfaces.Resolvers;
using Tasker.Domain.Entities.Application;
using Tasker.Domain.Exceptions;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Resolvers;

public class KanbanBoardResolver : IResolver<KanbanBoard, string>
{
    private readonly ApplicationContext _context;

    public KanbanBoardResolver(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<KanbanBoard> ResolveAsync(string id)
        => await _context.KanbanBoards.FirstOrDefaultAsync(b => b.Id == id)
           ?? throw new InvalidEntityException($"KanbanBoard with id {id} doesnt exists");
}