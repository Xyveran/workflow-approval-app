using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Domain.Entities;

public class ApprovalAction
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public Guid UserId { get; set; }
    public Guid WorkflowStepId { get; set; }
    public ApprovalActionType Action { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}