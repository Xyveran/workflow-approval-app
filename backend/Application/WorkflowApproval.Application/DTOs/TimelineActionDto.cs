using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Application.DTOs;

public class TimelineActionDto
{
    public Guid UserId { get; set; }
    public ApprovalActionType Action { get; set; }
    public string? Comments { get; set; }
    public DateTime Timestamp { get; set; }
}