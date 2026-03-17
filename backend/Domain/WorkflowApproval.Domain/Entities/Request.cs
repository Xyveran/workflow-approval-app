using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Domain.Entities;

public class Request
{
    public Guid Id { get; set; }
    public Guid RequestTypeId { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid WorkflowDefinitionId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public decimal? Amount { get; set; }
    public RequestStatus Status { get; set; }
    public int CurrentStep { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;
    public ICollection<ApprovalAction> ApprovalActions { get; set; } = new List<ApprovalAction>();
}