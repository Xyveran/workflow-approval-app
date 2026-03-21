using WorkflowApproval.Contracts.Enums;

namespace WorkflowApproval.Contracts.Responses;

public class RequestTimelineContract
{
    public Guid RequestId { get; set; }
    public RequestStatus Status { get; set; }
    public int CurrentStep { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ApprovalActionContract> Timeline { get; set; } = new();
    public List<TimelineStepContract> Steps { get; set; } = new();
}