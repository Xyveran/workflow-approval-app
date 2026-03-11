namespace WorkflowApproval.Domain.Entities;

public class WorkflowCondition
{
    public Guid Id { get; set; }
    public Guid WorkflowStepId { get; set; }
    public bool RequireAllApprovals { get; set; }
    public string? FieldName { get; set; }
    public string? Operator { get; set; }
    public string? Value { get; set; }
}