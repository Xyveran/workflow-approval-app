namespace WorkflowApproval.Domain.Entities;

public class WorkflowDefinition
{
    public Guid Id { get; set; }
    public Guid RequestTypeId { get; set; }
    public string Name { get; set; }
    public bool isActive { get; set; }
    public List<WorkflowStep> Steps { get; set; }
}