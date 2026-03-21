using WorkflowApproval.Contracts.Enums;

namespace WorkflowApproval.Contracts.Responses;

public class TimelineStepContract
{
    public int StepOrder { get; set; }
    public Guid RoleId { get; set; }
    public StepStatus Status { get; set; }
    public DateTime CompletedAt { get; set; }
    public List<TimelineActionContract> Actions { get; set; } = new();
}