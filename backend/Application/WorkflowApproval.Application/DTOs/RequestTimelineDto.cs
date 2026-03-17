using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Application.DTOs;

public class RequestTimelineDto
{
    public Guid RequestId { get; set; }
    public RequestStatus Status { get; set; }
    public int CurrentStep { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ApprovalActionDto> Timeline { get; set; } = new();
    public List<TimelineStepDto> Steps { get; set; } = new();
}