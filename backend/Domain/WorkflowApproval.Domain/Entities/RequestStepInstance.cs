using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Domain.Entities;

public class RequestStepInstance
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public Guid WorkflowStepId { get; set; }
    public StepStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}