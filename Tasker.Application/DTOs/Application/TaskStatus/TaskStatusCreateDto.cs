using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasker.Application.DTOs.Application.TaskStatus
{
    public class TaskStatusCreateDto
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public string? KanbanBoardId { get; set; }
    }
}
