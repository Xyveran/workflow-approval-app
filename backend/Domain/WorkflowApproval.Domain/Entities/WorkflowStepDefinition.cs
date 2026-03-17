using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Domain.Entities;

public class WorkflowStepDefinition
{
    public Guid Id { get; set; }
    public Guid WorkflowDefinitionId { get; set; }
    public WorkflowDefinition WorkflowDefinition { get; set; } = null!;
    public required string Name { get; set; }
    public int StepOrder { get; set; }
    public Guid RoleId { get; set; }
    public StepType StepType { get; set; }
    public bool AllowParallelApprovals { get; set; }
    public ICollection<WorkflowRule> Rules { get; set; } 
        = new List<WorkflowRule>();
    public ICollection<WorkflowCondition> Conditions { get; set; } 
        = new List<WorkflowCondition>();
}