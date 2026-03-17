namespace WorkflowApproval.Application.DTOs;
 
public class StepApprovalStatsDto
{
    public int StepOrder { get; set; }
    public int Approvals { get; set; }
    public int Rejections { get; set; }
}