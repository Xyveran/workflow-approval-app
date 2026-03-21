using WorkflowApproval.Contracts.Enums;

namespace WorkflowApproval.Contracts.Responses;

public class TimelineActionContract
{
    public Guid UserId { get; set; }
    public ApprovalActionType Action { get; set; }
    public string? Comments { get; set; }
    public DateTime Timestamp { get; set; }
}