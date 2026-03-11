using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Application.DTOs;
using WorkflowApproval.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Application.Services;


public class WorkflowService : IWorkflowService
{
    private readonly IWorkflowDbContext _dbContext;

    public WorkflowService(IWorkflowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Guid> SubmitRequest(CreateRequestDto dto)
    {
        // Find the workflow for the request type
        var workflow = await _dbContext.WorkflowDefinitions
            .Include(w => w.Steps.OrderBy(s=> s.StepOrder))
            .FirstOrDefaultAsync(w => w.RequestTypeId == dto.RequestTypeId);

        if (workflow == null)
            throw new InvalidOperationException("No workflow defined for this request type");

        var request = new Request
        {
            Id = Guid.NewGuid(),
            RequestTypeId = dto.RequestTypeId,
            Status = StepStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            CurrentStep = 1,
        };

        _dbContext.Requests.Add(request);
        await _dbContext.SaveChangesAsync();

        return request.Id;
    }
    public async Task ApproveRequest(Guid requestId, Guid userId, string? comments = null)
    {
        var request = await _dbContext.Requests
            .Include(r => r.ApprovalActions)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null) throw new InvalidOperationException("Request not found");
        if (request.Status != StepStatus.Pending) throw new InvalidOperationException("Request is not pending");


        // Get current workflow step
        var workflow = await _dbContext.WorkflowDefinitions
            .Include(w => w.Steps)
            .FirstOrDefaultAsync(w => w.RequestTypeId == request.RequestTypeId);

        var currentStep = workflow.Steps.FirstOrDefault(s => s.StepOrder == request.CurrentStep);
        if (currentStep == null)
            throw new InvalidOperationException("Current workflow step not found");

        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        // Check user's role
        if (user.RoleId != currentStep.RoleId)
            throw new UnauthorizedAccessException("User not authorized for this step");

        // Log approval
        _dbContext.ApprovalActions.Add(new ApprovalAction
        {
            Id = Guid.NewGuid(),
            RequestId = request.Id,
            UserId = userId,
            StepOrder = currentStep.StepOrder,
            Action = ApprovalActionType.Approved,
            Comments = comments,
            ActionDate = DateTime.UtcNow
        });

        // Move to next step or complete
        var nextStep = workflow.Steps.FirstOrDefault(s => s.StepOrder == currentStep.StepOrder + 1);

        if (nextStep != null)
        {
            request.CurrentStep = nextStep.StepOrder;
        }
        else
        {
           request.Status = StepStatus.Approved; 
        }

        await _dbContext.SaveChangesAsync();
    }
    public async Task RejectRequest(Guid requestId, Guid userId, string? comments = null)
    {
        var request = await _dbContext.Requests
            .Include(r => r.ApprovalActions)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null) throw new InvalidOperationException("Request not found");
        if (request.Status != StepStatus.Pending)
            throw new InvalidOperationException("Request is not pending");

        var workflow = await _dbContext.WorkflowDefinitions
            .Include(w => w.Steps)
            .FirstOrDefaultAsync(w => w.RequestTypeId == request.RequestTypeId);

        var currentStep = workflow.Steps.FirstOrDefault(s => s.StepOrder == request.CurrentStep);
        if (currentStep == null)
            throw new InvalidOperationException("Current workflow step not found");

        var user = await _dbContext.Users.FindAsync(userId);
        if (user.RoleId != currentStep.RoleId)
            throw new UnauthorizedAccessException("User not authorized for this step");

        _dbContext.ApprovalActions.Add(new ApprovalAction
        {
            Id = Guid.NewGuid(),
            RequestId = request.Id,
            UserId = userId,
            StepOrder = currentStep.StepOrder,
            Action = ApprovalActionType.Rejected,
            Comments = comments,
            ActionDate = DateTime.UtcNow
        });

        request.Status = StepStatus.Rejected;

        await _dbContext.SaveChangesAsync();
    }

    public async Task CommentOnRequest(Guid requestId, Guid userId, string comment)
    {
        var request = await _dbContext.Requests.FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null) throw new InvalidOperationException("Request not found");

        _dbContext.ApprovalActions.Add(new ApprovalAction
        {
            Id = Guid.NewGuid(),
            RequestId = requestId,
            UserId = userId,
            StepOrder = request.CurrentStep,
            Action = ApprovalActionType.Commented,
            Comments = comment,
            ActionDate = DateTime.UtcNow
        });

        await _dbContext.SaveChangesAsync();
    }
}