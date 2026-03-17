using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Application.DTOs;

public class TimelineStepDto
{
    public int StepOrder { get; set; }
    public Guid RoleId { get; set; }
    public StepStatus Status { get; set; }
    public DateTime CompletedAt { get; set; }
    public List<TimelineActionDto> Actions { get; set; } = new();
}