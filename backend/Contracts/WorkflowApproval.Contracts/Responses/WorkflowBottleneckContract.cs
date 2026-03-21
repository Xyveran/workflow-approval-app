namespace WorkflowApproval.Contracts.Responses;

public class WorkflowBottleneckContract
{
    public List<StepDurationContract> SlowestSteps { get; set; } = new();
    public List<StepRejectionContract> MostRejectedSteps { get; set; } = new();
}