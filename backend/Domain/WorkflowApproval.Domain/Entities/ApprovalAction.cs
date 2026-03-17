using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Domain.Entities;

public class ApprovalAction
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public Request Request { get; set; } = null!;
    public Guid UserId { get; set; }
    public ApprovalActionType Action { get; set; }
    public int StepOrder { get; set; }
    public string? Comments { get; set; }
    public DateTime ActionDate { get; set; }
    public DateTime CreatedAt { get; set; }
}