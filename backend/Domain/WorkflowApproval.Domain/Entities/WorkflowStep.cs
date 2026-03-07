using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Domain.Entities;

public class WorkflowStep
{
    public Guid Id { get; set; }
    public Guid WorkflowDefinitionId { get; set; }
    public string Name { get; set; }
    public string RoleRequired { get; set; }
    public StepType StepType { get; set; }
    public int StepOrder { get; set; }
    public bool AllowParallelApprovals { get; set; }
    public List<WorkflowCondition> Conditions { get; set; }
}