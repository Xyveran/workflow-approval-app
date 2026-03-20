using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Application.DTOs;
using WorkflowApproval.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using WorkflowApproval.Domain.Enums;
using Microsoft.VisualBasic;
using WorkflowApproval.Application.Workflow;

namespace WorkflowApproval.Application.Services;


public class AnalyticsService
{
    private readonly IWorkflowDbContext _dbContext;
    public AnalyticsService(IWorkflowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<WorkflowAnalyticsDto> GetWorkflowAnalytics()
    {
        var requests = await _dbContext.Requests.ToListAsync();
        var approvals = await _dbContext.ApprovalActions.ToListAsync();

        var approved = requests.Count(r => r.Status == RequestStatus.Approved);
        var rejected = requests.Count(r => r.Status == RequestStatus.Rejected);

        var avgTime = requests
            .Where(r => r.Status == RequestStatus.Approved && r.CompletedAt.HasValue)
            .Select(r => (r.CompletedAt!.Value - r.CreatedAt).TotalHours)
            .DefaultIfEmpty(0)
            .Average();

        var stepStats = approvals
            .GroupBy(a => a.StepOrder)
            .Select(g => new StepApprovalStatsDto
            {
                StepOrder = g.Key,
                Approvals = g.Count(a => a.Action == ApprovalActionType.Approved),
                Rejections = g.Count(a => a.Action == ApprovalActionType.Rejected)
            }).ToList();

        return new WorkflowAnalyticsDto
        {
            TotalRequests = requests.Count,
            ApprovedRequests = approved,
            RejectedRequests = rejected,
            AverageApprovalHours = avgTime,
            StepStats = stepStats
        };
    }

    public async Task<WorkflowBottleneckDto> GetWorkflowBottlenecks()
    {
        var actions = await _dbContext.ApprovalActions.ToListAsync();

        var stepDurations = actions
            .Where(a => a.Action == ApprovalActionType.Approved)
            .GroupBy(a => a.StepOrder)
            .Select(g => new StepDurationDto
            {
                StepOrder = g.Key,
                AverageHours = g.Average(a =>
                    (a.ActionDate - actions
                        .Where(x => x.RequestId == a.RequestId && x.StepOrder == a.StepOrder)
                        .Min(x => x.ActionDate))
                        .TotalHours)
            })
            .OrderByDescending(x => x.AverageHours)
            .Take(5)
            .ToList();

        var rejectedSteps = actions
            .Where(a => a.Action == ApprovalActionType.Rejected)
            .GroupBy(a => a.StepOrder)
            .Select(g => new StepRejectionDto
            {
                StepOrder = g.Key,
                Rejections = g.Count()
            })
            .OrderByDescending(x => x.Rejections)
            .Take(5)
            .ToList();

        return new WorkflowBottleneckDto
        {
            SlowestSteps = stepDurations,
            MostRejectedSteps = rejectedSteps
        };
    }
}