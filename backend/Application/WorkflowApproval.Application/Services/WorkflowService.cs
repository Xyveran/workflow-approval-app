using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Application.DTOs;
using WorkflowApproval.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WorkflowApproval.Domain.Enums;
using WorkflowApproval.Application.Workflow;

namespace WorkflowApproval.Application.Services;


public class WorkflowService : IWorkflowService
{
    private readonly IWorkflowDbContext _dbContext;
    private readonly WorkflowExecutionEngine _workflowEngine;
    private readonly AnalyticsService _analyticsService;

    public WorkflowService(
        IWorkflowDbContext dbContext,
        WorkflowExecutionEngine workflowEngine,
        AnalyticsService analyticsService)
    {
        _dbContext = dbContext;
        _workflowEngine = workflowEngine;
        _analyticsService = analyticsService;
    }
    public async Task<Guid> SubmitRequest(CreateRequestDto dto)
    {
        var workflow = await _dbContext.WorkflowDefinitions
            .Where(w => w.IsActive && w.RequestTypeId == dto.RequestTypeId)
            .OrderByDescending(w => w.Version)
            .FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("No active workflow found for this request type.");

        var request = new Request
        {
            Id = Guid.NewGuid(),
            RequestTypeId = dto.RequestTypeId,
            WorkflowDefinitionId = workflow.Id,
            Title = dto.Title,
            Description = dto.Description,
            Amount = dto.Amount,
            Status = RequestStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            CurrentStep = 1
        };

        _dbContext.Requests.Add(request);

        await _workflowEngine.InitializeWorkflow(request.Id, dto.RequestTypeId);

        await _dbContext.SaveChangesAsync();

        return request.Id;
    }
    public async Task<bool> ApproveRequest(Guid requestId, Guid userId, string? comments = null)
    {
        try
        {
            var request = await _dbContext.Requests.FindAsync(requestId);

            if (request == null) return false;

            _dbContext.ApprovalActions.Add(new ApprovalAction
            {
                Id = Guid.NewGuid(),
                RequestId = requestId,
                UserId = userId,
                StepOrder = request.CurrentStep,
                Action = ApprovalActionType.Approved,
                Comments = comments,
                ActionDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            });

            await _workflowEngine.AdvanceStep(requestId);

            var instance = await _dbContext.WorkflowInstances
                .FirstOrDefaultAsync(w => w.RequestId == requestId);
 
            if (instance?.Status == StepStatus.Approved)
            {
                request.Status = RequestStatus.Approved;
                request.CompletedAt = DateTime.UtcNow;
            }
 
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }
    public async Task<bool> RejectRequest(Guid requestId, Guid userId, string? comments = null)
    {
        try
        {
            var request = await _dbContext.Requests.FindAsync(requestId);
            if (request == null) return false;

            _dbContext.ApprovalActions.Add(new ApprovalAction
            {
                Id = Guid.NewGuid(),
                RequestId = requestId,
                UserId = userId,
                StepOrder = request.CurrentStep,
                Action = ApprovalActionType.Rejected,
                Comments = comments,
                ActionDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            });

            await _workflowEngine.RejectStep(requestId);

            request.Status = RequestStatus.Rejected;
            request.CompletedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }        
    }

    public async Task<bool> CommentOnRequest(Guid requestId, Guid userId, string comment)
    {
        var request = await _dbContext.Requests.FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null) return false;

        _dbContext.ApprovalActions.Add(new ApprovalAction
        {
            Id = Guid.NewGuid(),
            RequestId = requestId,
            UserId = userId,
            StepOrder = request.CurrentStep,
            Action = ApprovalActionType.Commented,
            Comments = comment,
            ActionDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<RequestTimelineDto?> GetRequestTimeline(Guid requestId)
    {
        var request = await _dbContext.Requests
            .Include(r => r.ApprovalActions)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null)
            return null;

        var workflowInstance = await _dbContext.WorkflowInstances
            .Include(w => w.Steps)
            .FirstOrDefaultAsync(w => w.RequestId == requestId);

        var timelineSteps = workflowInstance?.Steps
            .OrderBy(s => s.StepOrder)
            .Select(step => new TimelineStepDto
            {
                StepOrder = step.StepOrder,
                RoleId = step.RoleId,
                Status = step.Status,
                CompletedAt = step.CompletedAt
            }).ToList() ?? new List<TimelineStepDto>();

        return new RequestTimelineDto
        {
            RequestId = request.Id,
            Status = request.Status,
            CurrentStep = request.CurrentStep,
            CreatedAt = request.CreatedAt,
            Steps = timelineSteps,
            Timeline = request.ApprovalActions?
                .OrderBy(a => a.ActionDate)
                .Select(a => new ApprovalActionDto
                {
                    UserId = a.UserId,
                    StepOrder = a.StepOrder,
                    Action = a.Action.ToString(),
                    Comments = a.Comments,
                    ActionDate = a.ActionDate
                })
                .ToList() ?? new List<ApprovalActionDto>(),
        };
    }

    public async Task<List<RequestTimelineDto>> GetPendingRequests(Guid roleId)
    {
        var pendingRequestIds = await _dbContext.WorkflowInstances
            .Include(w => w.Steps)
            .Where(w => w.Steps.Any(s => s.Status == StepStatus.Pending && s.RoleId == roleId))
            .Select(w => w.RequestId)
            .ToListAsync();

        var requests = await _dbContext.Requests
            .Where(r => pendingRequestIds.Contains(r.Id))
            .ToListAsync();

        return requests.Select(r => new RequestTimelineDto
        {
            RequestId = r.Id,
            Status = r.Status,
            CurrentStep = r.CurrentStep,
            CreatedAt = r.CreatedAt
        }).ToList();
    }

    public Task<WorkflowAnalyticsDto> GetWorkflowAnalytics() =>
        _analyticsService.GetWorkflowAnalytics();
    public Task<WorkflowBottleneckDto> GetWorkflowBottlenecks() =>
        _analyticsService.GetWorkflowBottlenecks();
}