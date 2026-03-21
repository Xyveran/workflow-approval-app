using Microsoft.EntityFrameworkCore;
using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Domain.Entities;
using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Application.Workflow;

public class WorkflowExecutionEngine
{
    private readonly IWorkflowDbContext _dbContext;

    public WorkflowExecutionEngine(IWorkflowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WorkflowInstance> InitializeWorkflow(Guid requestId, Guid requestTypeId)
    {
        var workflow = await _dbContext.WorkflowDefinitions
            .Include(w => w.Steps.OrderBy(s => s.StepOrder))
            .FirstOrDefaultAsync(w => w.RequestTypeId == requestTypeId && w.IsActive)
            ?? throw new InvalidOperationException("No active workflow defined for this request type.");

        var instance = new WorkflowInstance
        {
            Id = Guid.NewGuid(),
            RequestId = requestId,
            WorkflowDefinitionId = workflow.Id,
            Status = StepStatus.Pending,
            StartedAt = DateTime.UtcNow
        };

        _dbContext.WorkflowInstances.Add(instance);

        foreach (var step in workflow.Steps)
        {
            _dbContext.WorkflowStepInstances.Add(new WorkflowStepInstance
            {
                Id = Guid.NewGuid(),
                WorkflowInstanceId = instance.Id,
                WorkflowStepId = step.Id,
                RoleId = step.RoleId,
                StepOrder = step.StepOrder,
                Status = step.StepOrder == 1 ? StepStatus.Pending : StepStatus.NotStarted
            });
        }

        return instance;
    }

    public async Task AdvanceStep(Guid requestId)
    {
        var instance = await _dbContext.WorkflowInstances
            .Include(w => w.Steps)
            .FirstOrDefaultAsync(w => w.RequestId == requestId)
            ?? throw new InvalidOperationException("Request not found.");

        var request = await _dbContext.Requests.FindAsync(requestId)
            ?? throw new InvalidOperationException("Request not found.");

        var current = instance.Steps
            .FirstOrDefault(s => s.Status == StepStatus.Pending)
            ?? throw new InvalidOperationException("No active step found.");

        current.Status = StepStatus.Approved;
        current.CompletedAt = DateTime.UtcNow;

        var remainingSteps = instance.Steps
            .Where(s => s.StepOrder > current.StepOrder)
            .OrderBy(s => s.StepOrder)
            .ToList();

        foreach (var step in remainingSteps)
        {
            var rules = await _dbContext.WorkflowRules
                .Where(r => r.WorkflowStepId == step.WorkflowStepId)
                .ToListAsync();

            bool stepApplies = !rules.Any() || 
                rules.All(r => WorkflowRuleEvaluator.Evaluate(request.Amount ?? 0, r));

            if (stepApplies)
            {
                step.Status = StepStatus.Pending;
                request.CurrentStep = step.StepOrder;
                return;
            }
            step.Status = StepStatus.Skipped;
        }
        
        instance.Status = StepStatus.Approved;
        request.CurrentStep = current.StepOrder;
    }

    public async Task RejectStep(Guid requestId)
    {
        var instance = await _dbContext.WorkflowInstances
            .Include(w => w.Steps)
            .FirstOrDefaultAsync(w => w.RequestId == requestId)
            ?? throw new InvalidOperationException("No workflow instance found for this request.");

        var current = instance.Steps
            .FirstOrDefault(s => s.Status == StepStatus.Pending)
            ?? throw new InvalidOperationException("No active step found.");

        current.Status = StepStatus.Rejected;
        current.CompletedAt = DateTime.UtcNow;
        instance.Status = StepStatus.Rejected;
    }

    // public async Task<bool> StepIsValid(Guid stepId, Request request)
    // {
        
    // }
}