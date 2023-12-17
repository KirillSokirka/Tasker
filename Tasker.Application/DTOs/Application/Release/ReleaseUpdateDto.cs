using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasker.Application.DTOs.Application.Release;

public class ReleaseUpdateDto
{
    public required string Id { get; set; }
    public string? Title { get; set; }
    public required string ProjectId { get; set; }
    public bool? IsReleased { get; set; }
    public DateTime? EndDate { get; set; }
}