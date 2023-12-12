using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs;
using Tasker.Application.DTOs.Application.KanbanBoard;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Application.Resolvers.Interfaces;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.Repositories
{
    public class KanbanBoardRepository : IKanbanBoardRepository
    {
        private readonly IResolver<Project, string> _projectResolver;
        private readonly IResolver<TaskStatus, string> _statusResolver;
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public KanbanBoardRepository(ApplicationContext context, IMapper mapper,
            IResolver<Project, string> projectResolver,
            IResolver<TaskStatus, string> statusResolver)
        {
            _projectResolver = projectResolver;
            _statusResolver = statusResolver;
            _context = context;
            _mapper = mapper;
        }

        public async Task<KanbanBoardDto?> CreateAsync(KanbanBoardCreateDto dto)
        {
            if (await _context.KanbanBoards.AnyAsync(p => p.Title == dto.Title))
            {
                return null;
            }

            var board = new KanbanBoard() 
            { 
                Id = Guid.NewGuid().ToString(),
                Title = dto.Title, 
                Project = await _projectResolver.ResolveAsync(dto.ProjectId),
                ProjectId = dto.ProjectId,
                Columns = dto.ColumnIds.Select(colId => _statusResolver.ResolveAsync(colId).Result).ToList()
            };
            
            await _context.KanbanBoards.AddAsync(board);
            await _context.SaveChangesAsync();

            return _mapper.Map<KanbanBoardDto>(board);
        }

        public async Task<KanbanBoardDto?> UpdateAsync(KanbanBoardDto dto)
        {  
            var board = await _context.KanbanBoards.FindAsync(dto.Id);
            
            if (board is null)
            {
                return null;
            }

            _mapper.Map(dto, board);

            await _context.SaveChangesAsync();

            return _mapper.Map<KanbanBoardDto>(board);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var board = await _context.KanbanBoards.FindAsync(id);
            
            if (board is null)
            {
                return false;
            }

            _context.KanbanBoards.Remove(board);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<KanbanBoardDto?> GetAsync(string id)
        {
            var board = await GetEntity(id);
            
            return board is not null ? _mapper.Map<KanbanBoardDto>(board) : null;
        }

        private async Task<KanbanBoard?> GetEntity(string id)
            => await _context.KanbanBoards
            .AsNoTracking()
            .Include(t => t.Project)
            .Include(t => t.Columns)
            .AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<KanbanBoardDto>?> GetAllAsync()
        {
            var boards = new List<KanbanBoardDto>();
            await _context.KanbanBoards.AsNoTracking().Include(t => t.Project)
            .Include(t => t.Columns).ForEachAsync(b => boards.Add(_mapper.Map<KanbanBoardDto>(b)));

            return boards.Any() ? boards : null;
        }
    }

}