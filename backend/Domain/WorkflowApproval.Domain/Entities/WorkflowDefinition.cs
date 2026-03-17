namespace WorkflowApproval.Domain.Entities;

public class WorkflowDefinition
{
    public Guid Id { get; set; }
    public Guid RequestTypeId { get; set; }
    public required string Name { get; set; }
    public bool IsActive { get; set; }
    public int Version { get; set; }
    public RequestType? RequestType { get; set; }
    public ICollection<WorkflowStepDefinition> Steps { get; set; } 
        = new List<WorkflowStepDefinition>();
}