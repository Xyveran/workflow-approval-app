namespace WorkflowApproval.Application.DTOs;

public class WorkflowBottleneckDto
{
    public List<StepDurationDto> SlowestSteps { get; set; } = new();
    public List<StepRejectionDto> MostRejectedSteps { get; set; } = new();
}