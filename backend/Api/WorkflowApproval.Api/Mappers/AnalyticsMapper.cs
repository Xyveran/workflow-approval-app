using Namotion.Reflection;
using WorkflowApproval.Application.DTOs;
using WorkflowApproval.Contracts.Responses;

namespace WorkflowApproval.Api.Mappers;

public static class AnalyticsMapper
{
    public static WorkflowAnalyticsContract ToContract(this WorkflowAnalyticsDto dto) =>
        new()
        {
            TotalRequests = dto.TotalRequests,
            ApprovedRequests = dto.ApprovedRequests,
            RejectedRequests = dto.RejectedRequests,
            AverageApprovalHours = dto.AverageApprovalHours,
            StepStats = dto.StepStats.Select(s => s.ToContract()).ToList()
        };

    public static StepApprovalStatsContract ToContract(this StepApprovalStatsDto dto) =>
        new()
        {
            StepOrder = dto.StepOrder,
            Approvals = dto.Approvals,
            Rejections = dto.Rejections
        };

    public static WorkflowBottleneckContract ToContract(this WorkflowBottleneckDto dto) =>
        new()
        {
            SlowestSteps = dto.SlowestSteps.Select(s => s.ToContract()).ToList(),
            MostRejectedSteps = dto.MostRejectedSteps.Select(s => s.ToContract()).ToList()
        };

    public static StepDurationContract ToContract(this StepDurationDto dto) =>
        new()
        {
            StepOrder = dto.StepOrder,
            AverageHours = dto.AverageHours
        };

    public static StepRejectionContract ToContract(this StepRejectionDto dto) =>
        new()
        {
            StepOrder = dto.StepOrder,
            Rejections = dto.Rejections
        };
}