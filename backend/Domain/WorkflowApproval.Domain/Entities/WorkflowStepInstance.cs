using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Domain.Entities;

public class WorkflowStepInstance
{
    public Guid Id { get; set; }

    public Guid WorkflowInstanceId { get; set; }
    public Guid WorkflowStepId { get; set; }
    public Guid RoleId { get; set; }

    public int StepOrder { get; set; }

    public string RoleRequired { get; set; } = string.Empty;

    public StepStatus Status { get; set; } = StepStatus.Pending;

    public DateTime CompletedAt { get; set; }
}