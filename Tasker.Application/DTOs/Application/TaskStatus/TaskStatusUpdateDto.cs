using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasker.Application.DTOs.Application.TaskStatus
{
    public class TaskStatusUpdateDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? KanbanBoardId { get; set; }
    }
}
