namespace WorkflowApproval.Application.DTOs;

public class WorkflowAnalyticsDto
{
    public int TotalRequests { get; set; }

    public int ApprovedRequests { get; set; }

    public int RejectedRequests { get; set; }

    public double AverageApprovalHours { get; set; }

    public List<StepApprovalStatsDto> StepStats { get; set; } = new();
}