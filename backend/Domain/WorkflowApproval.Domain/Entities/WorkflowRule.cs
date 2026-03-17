namespace WorkflowApproval.Domain.Entities;

public class WorkflowRule
{
    public Guid Id { get; set; }
    public Guid WorkflowStepId { get; set; }
    public required string Field { get; set; }
    public required string Operator { get; set; }
    public required string Value { get; set; }
}