using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Domain.Entities;

public class Request
{
    public Guid Id { get; set; }
    public Guid RequestTypeId { get; set; }
    public Guid CreatedBy { get; set; }
    public StepStatus Status { get; set; }
    public int CurrentStep { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<ApprovalAction>? ApprovalActions { get; set; }
}