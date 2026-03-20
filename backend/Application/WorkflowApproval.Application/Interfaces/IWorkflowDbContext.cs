using Microsoft.EntityFrameworkCore;
using WorkflowApproval.Domain.Entities;

namespace WorkflowApproval.Application.Interfaces;

public interface IWorkflowDbContext
{
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<Request> Requests { get; }
    DbSet<RequestType> RequestTypes { get; }
    DbSet<WorkflowDefinition> WorkflowDefinitions { get; }
    DbSet<WorkflowStepDefinition> WorkflowStepDefinitions { get; }
    DbSet<WorkflowStepInstance> WorkflowStepInstances { get; }
    DbSet<WorkflowInstance> WorkflowInstances { get; }

    DbSet<ApprovalAction> ApprovalActions { get; }
    DbSet<WorkflowRule> WorkflowRules { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}