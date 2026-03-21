namespace WorkflowApproval.Contracts.Responses;
 
public class StepApprovalStatsContract
{
    public int StepOrder { get; set; }
    public int Approvals { get; set; }
    public int Rejections { get; set; }
}