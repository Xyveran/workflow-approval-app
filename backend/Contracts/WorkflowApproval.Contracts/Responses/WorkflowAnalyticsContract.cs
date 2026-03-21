namespace WorkflowApproval.Contracts.Responses;

public class WorkflowAnalyticsContract
{
    public int TotalRequests { get; set; }
    public int ApprovedRequests { get; set; }
    public int RejectedRequests { get; set; }
    public double AverageApprovalHours { get; set; }
    public List<StepApprovalStatsContract> StepStats { get; set; } = new();
}