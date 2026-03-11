namespace WorkflowApproval.Domain.Entities;

public class RequestType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<WorkflowDefinition> Workflows { get; set; }

}