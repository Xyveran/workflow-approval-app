namespace WorkflowApproval.Domain.Entities;

public class WorkflowDefinition
{
    public Guid Id { get; set; }
    public Guid RequestTypeId { get; set; }
    public required string Name { get; set; }
    public bool isActive { get; set; }
    public RequestType? RequestType { get; set; }
    public ICollection<WorkflowStep>? Steps { get; set; }
}