namespace WorkflowApproval.Contracts.Responses;

public class StepDurationContract
{
    public int StepOrder { get; set; }
    public double AverageHours { get; set; }
}

public class StepRejectionContract
{
    public int StepOrder { get; set; }
    public int Rejections { get; set; }
}