using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Domain.Entities;

public class WorkflowInstance
{
    public Guid Id { get; set; }

    public Guid RequestId { get; set; }

    public Guid WorkflowDefinitionId { get; set; }

    public StepStatus Status { get; set; } = StepStatus.Pending;

    public DateTime StartedAt { get; set; }

    public List<WorkflowStepInstance> Steps { get; set; } = new();
}