﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tasker.Application.DTOs;
using Tasker.Application.Interfaces.Repositories;
using Tasker.Domain.Entities.Application;
using Tasker.Infrastructure.Data.Application;

namespace Tasker.Application.Repositories
{
    public class KanbanBoardRepository : IKanbanBoardRepository
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public KanbanBoardRepository(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<KanbanBoardDto?> CreateAsync(KanbanBoardDto KanbanBoardDto)
        {
            if (await _context.KanbanBoards.AnyAsync(p => p.Title == KanbanBoardDto.Title))
            {
                return null;
            }

            var board = _mapper.Map<KanbanBoard>(KanbanBoardDto);
            board.Id = Guid.NewGuid().ToString();

            await _context.KanbanBoards.AddAsync(board);
            await _context.SaveChangesAsync();

            return _mapper.Map<KanbanBoardDto>(board);
        }

        public async Task<KanbanBoardDto?> UpdateAsync(string id, KanbanBoardDto KanbanBoardDto)
        {
            KanbanBoardDto.Id = id;
            
            var board = await _context.KanbanBoards.FindAsync(KanbanBoardDto.Id);
            
            if (board is null)
            {
                return null;
            }

            _mapper.Map(KanbanBoardDto, board);

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
            var board = await _context.KanbanBoards.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            
            return board is not null ? _mapper.Map<KanbanBoardDto>(board) : null;
        }
    }

}